using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthBar : StatsBar_HUD
{
    protected override void SetPercentText()
    {
        percentText.text = (targetFillAmout * 100f).ToString("f2") + "%";
    }
}
