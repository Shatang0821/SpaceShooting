using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionFire : PlayerFire
{
    protected override void OnEnable()
    {
        base.OnEnable();
        CanLaunchMissile = false; // 禁止发射导弹
    }
}
