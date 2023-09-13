using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour
{
    [SerializeField] Image fillImageBack;

    [SerializeField] Image fillImageFront;

    [SerializeField] bool delayFille = true;

    [SerializeField] float fillDelay = 0.5f;

    [SerializeField] float fillSpeed = 0.1f;

    float currentFillAmout;

    protected float targetFillAmout;

    float t;

    WaitForSeconds waitForeDelayFill;

    Coroutine bufferedFillingCoroutine;

    Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;

        waitForeDelayFill = new WaitForSeconds(fillDelay);
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
            yield return waitForeDelayFill;
        }
        t = 0;

        while(t < 1f)
        {
            t += Time.deltaTime * fillSpeed;
            currentFillAmout = Mathf.Lerp(currentFillAmout, targetFillAmout, t);
            image.fillAmount = currentFillAmout;

            yield return null;
        }
        
    }

}
