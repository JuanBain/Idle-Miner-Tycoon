using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseManagerLocation : BaseManagerLocation
{
    public static Action<WarehouseManagerLocation> OnBoost;

    public override void RunBoost()
    {
        OnBoost?.Invoke(this);
    }
}