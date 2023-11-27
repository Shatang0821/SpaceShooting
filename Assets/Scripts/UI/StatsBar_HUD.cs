using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsBar_HUD : StatsBar
{
    [SerializeField] protected Text percentText;

    protected virtual void SetPercentText()
    {
        percentText.text = Mathf.RoundToInt(targetFillAmout * 100f) + "%" ;
    }

    public override void Initialize(float currentValue, float maxValue)
    {
        base.Initialize(currentValue, maxValue);
        SetPercentText();
    }

    protected override IEnumerator BufferedFillingCoroutine(Image image)
    {
        SetPercentText();
        return base.BufferedFillingCoroutine(image);
    }
}
