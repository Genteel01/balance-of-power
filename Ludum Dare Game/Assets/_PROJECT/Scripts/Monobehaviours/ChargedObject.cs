using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChargedObject : BaseChargeLevel
{


    public UnityEvent lowChargeEvent;
    public UnityEvent highChargeEvent;


    public override void UpdateCharge(float change)
    {
        base.UpdateCharge(change);
    }

    public override void HandleMinCharge()
    {
        lowChargeEvent.Invoke();
    }

    public override void HandleMaxCharge()
    {
        highChargeEvent.Invoke();
    }
}
