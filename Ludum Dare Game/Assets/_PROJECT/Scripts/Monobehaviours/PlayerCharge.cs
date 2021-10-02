using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharge : BaseChargeLevel
{
    public bool suckPower;

    public GameObject zapLocationObject;

    public Transform zapStartLocation;

    public float zapRange = 5;

    public GameObject playerModel;

    private void Start()
    {
        base.Start();
        zapLocationObject.SetActive(false);
    }
    public override void HandleMinCharge()
    {
        Debug.Log("You died from low charge");
    }

    public override void HandleMaxCharge()
    {
        Debug.Log("You died from overloading");
    }

    public override void UpdateCharge(float change)
    {
        base.UpdateCharge(change);
    }

    void OnSuckPower()
    {
        suckPower = true;
        playerModel.transform.rotation = Quaternion.Euler(0, 0, 180);
    }

    void OnSendPower()
    {
        suckPower = false;
        playerModel.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void OnZap(InputValue value)
    {
        Vector3 aimVal = value.Get<Vector2>();
        if(aimVal.magnitude < 0.15)
        {
            zapLocationObject.SetActive(false);
        }
        else
        {
            zapLocationObject.SetActive(true);
            aimVal *= zapRange;
            RaycastHit lineCastInfo;
            if (Physics.Linecast(zapStartLocation.position, zapStartLocation.position + aimVal, out lineCastInfo))
            {
                zapLocationObject.transform.position = lineCastInfo.point;
                ChargedObject otherObject = lineCastInfo.collider.GetComponentInParent<ChargedObject>();
                if (otherObject)
                {
                    float change = otherObject.transferSpeed * transferSpeed * 0.06f;
                    if(suckPower)
                    {
                        change = Mathf.Min(change, Mathf.Abs(otherObject.chargeLevel));
                        UpdateCharge(change);
                        otherObject.UpdateCharge(-change);
                    }
                    else
                    {
                        change = Mathf.Min(change, Mathf.Abs(otherObject.maxCharge - otherObject.chargeLevel));
                        UpdateCharge(-change);
                        otherObject.UpdateCharge(change);
                    }
                }

            }
            else
            {
                zapLocationObject.transform.position = zapStartLocation.position + aimVal;
            }
        }
    }
}
