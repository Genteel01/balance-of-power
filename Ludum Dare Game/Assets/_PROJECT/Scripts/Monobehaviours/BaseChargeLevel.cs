using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseChargeLevel : MonoBehaviour
{
    public float maxCharge = 1;

    public float chargeLevel;

    public Material fullMaterial;
    public Material emptyMaterial;

    public MeshRenderer[] chargeIndicator;

    List<Vector3> startingIndicatorSize = new List<Vector3>();

    public float transferSpeed;
    protected void Start()
    {
        for (int i = 0; i < chargeIndicator.Length; i++)
        {
            chargeIndicator[i].material.Lerp(emptyMaterial, fullMaterial, chargeLevel / maxCharge);
            startingIndicatorSize.Add(chargeIndicator[i].transform.localScale);
            chargeIndicator[i].transform.localScale = new Vector3(startingIndicatorSize[i].x, startingIndicatorSize[i].y, startingIndicatorSize[i].z * (chargeLevel / maxCharge));
        }
    }
    public virtual void UpdateCharge(float change)
    {
        chargeLevel += change;
        if(chargeLevel <= 0)
        {
            HandleMinCharge();
        }
        else if (chargeLevel >= maxCharge)
        {
            HandleMaxCharge();
        }
        for (int i = 0; i < chargeIndicator.Length; i++)
        {
            chargeIndicator[i].material.Lerp(emptyMaterial, fullMaterial, chargeLevel / maxCharge);
            chargeIndicator[i].transform.localScale = new Vector3(startingIndicatorSize[i].x, startingIndicatorSize[i].y, startingIndicatorSize[i].z * (chargeLevel / maxCharge));
        }
    }

    public abstract void HandleMaxCharge();

    public abstract void HandleMinCharge();
}
