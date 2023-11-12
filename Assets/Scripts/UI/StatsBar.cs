using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour
{ 
    //画像
    [SerializeField] Image fillImageBack;
    [SerializeField] Image fillImageFront;

    //
    [SerializeField] bool delayFille = true;

    //fillDelay時間の後から変化開始
    [SerializeField] float fillDelay = 0.5f;
    //変化速度
    [SerializeField] float fillSpeed = 0.1f;

    //計算用
    float currentFillAmout;

    protected float targetFillAmout;

    float previousFillAmount;

    float t;

    WaitForSeconds waitForDelayFill;

    Coroutine bufferedFillingCoroutine;

    Canvas canvas;

    private void Awake()
    {
        //Canvasが存在しているときだけ設定する
        if (TryGetComponent<Canvas>(out Canvas canvas))
        {
            canvas.worldCamera = Camera.main;
        }
        waitForDelayFill = new WaitForSeconds(fillDelay);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public virtual void Initialize(float currentValue,float maxValue)
    {
        //例えば HP この式でパーセントで表せます
        currentFillAmout = currentValue / maxValue;
        targetFillAmout = currentFillAmout;
        fillImageBack.fillAmount = currentFillAmout;
        fillImageFront.fillAmount = currentFillAmout;
    }

    public void UpdateStats(float currentValue, float maxValue)
    {
        targetFillAmout = currentValue / maxValue;

        if(bufferedFillingCoroutine != null)
        {
            StopCoroutine(bufferedFillingCoroutine);
        }
        // if stats reduce
        if(currentFillAmout > targetFillAmout)
        {
            // fill image front = target fill amout
            fillImageFront.fillAmount = targetFillAmout;
            // slowly reduce fill image back's fill amout
            bufferedFillingCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageBack));

            return;
        }
        // if stats increase
        if(currentFillAmout < targetFillAmout)
        {
            // fill image back's amout = target fill amout
            fillImageBack.fillAmount = targetFillAmout;
            // slowly increase fill image front's fill amout
            bufferedFillingCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageFront));
        }

    }

    protected virtual IEnumerator BufferedFillingCoroutine(Image image)
    {
        if(delayFille)
        {
            yield return waitForDelayFill;
        }
        previousFillAmount = currentFillAmout;
        t = 0;

        while(t < 1f)
        {
            t += Time.deltaTime * fillSpeed;
            currentFillAmout = Mathf.Lerp(previousFillAmount, targetFillAmout, t);
            image.fillAmount = currentFillAmout;

            yield return null;
        }
        
    }

}
