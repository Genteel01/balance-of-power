using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerCharge : BaseChargeLevel
{
    public bool suckPower;

    public GameObject zapLocationObject;

    public GameObject zapStartLocation;

    public float zapRange = 5;

    //Had to move this here and use Update instead of OnZap to make keyboard controls work
    public Vector3 aimValue = new Vector3();

    public UnityEvent loseEvent;

    private void Start()
    {
        base.Start();
        zapLocationObject.SetActive(false);
        zapStartLocation.SetActive(false);
    }
    public override void HandleMinCharge()
    {
        base.HandleMinCharge();
        loseEvent.Invoke();
        Debug.Log("You died from low charge");
    }

    public override void HandleMaxCharge()
    {
        base.HandleMaxCharge();
        loseEvent.Invoke();
        Debug.Log("You died from overloading");
    }

    public override void UpdateCharge(float change)
    {
        base.UpdateCharge(change);
    }

    void OnSuckPower()
    {
        suckPower = true;
    }

    void OnSendPower()
    {
        suckPower = false;
    }

    void OnZap(InputValue value)
    {
        aimValue = value.Get<Vector2>();
        aimValue *= zapRange;
    }

    private void Update()
    {
        if (aimValue.magnitude < 0.2)
        {
            zapLocationObject.SetActive(false);
            zapStartLocation.SetActive(false);
        }
        else
        {
            if (suckPower)
            {
                zapLocationObject.SetActive(true);
                zapStartLocation.SetActive(false);
            }
            else
            {
                zapStartLocation.SetActive(true);
                zapLocationObject.SetActive(false);
            }
            RaycastHit lineCastInfo;
            if (Physics.Linecast(zapStartLocation.transform.position, zapStartLocation.transform.position + aimValue, out lineCastInfo))
            {
                zapLocationObject.transform.position = lineCastInfo.point;
                ChargedObject otherObject = lineCastInfo.collider.GetComponentInParent<ChargedObject>();
                if (otherObject)
                {
                    float change = otherObject.transferSpeed * transferSpeed * Time.deltaTime;
                    if (suckPower)
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
                zapLocationObject.transform.position = zapStartLocation.transform.position + aimValue;
            }
        }
    }
}
