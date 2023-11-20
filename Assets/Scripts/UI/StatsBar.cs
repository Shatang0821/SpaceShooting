using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour
{ 
    //画像
    [SerializeField] Image fillImageBack;
    [SerializeField] Image fillImageFront;

    //遅れてからフィル値を変化させる
    [SerializeField] bool delayFille = true;

    //fillDelay時間の後から変化開始
    [SerializeField] float fillDelay = 0.5f;
    //変化速度
    [SerializeField] float fillSpeed = 0.1f;

    //計算用
    float currentFillAmout;

    //目標フィル値
    protected float targetFillAmout;

    //現在フィル値
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
        //例えば この式でパーセントで表せます
        //100/100 = 1つまり100%
        currentFillAmout = currentValue / maxValue;
        targetFillAmout = currentFillAmout;
        fillImageBack.fillAmount = currentFillAmout;
        fillImageFront.fillAmount = currentFillAmout;
    }

    public void UpdateStats(float currentValue, float maxValue)
    {
        //パーセント式に直す
        targetFillAmout = currentValue / maxValue;

        //コルーチンが実行している時止める
        if(bufferedFillingCoroutine != null)
        {
            StopCoroutine(bufferedFillingCoroutine);
        }
        // 目標値より大きい場合
        if(currentFillAmout > targetFillAmout)
        {
            //前のフィル値を目標フィル値に直す
            fillImageFront.fillAmount = targetFillAmout;
            // コルーチンを使って裏画像少しずつ変化させる
            bufferedFillingCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageBack));

            return;
        }
        // 目標値より小さい場合
        if(currentFillAmout < targetFillAmout)
        {
            // 裏のフィル値を目標フィル値に直す
            fillImageBack.fillAmount = targetFillAmout;
            // コルーチンを使って前画像少しずつ変化させる
            bufferedFillingCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageFront));
        }

    }

    /// <summary>
    /// フィル値を変化させるコルーチン
    /// </summary>
    /// <param name="image">変化させる画像</param>
    /// <returns></returns>
    protected virtual IEnumerator BufferedFillingCoroutine(Image image)
    {
        //待機時間あるなら
        if(delayFille)
        {
            //待つ
            yield return waitForDelayFill;
        }
        //今のフィル値を更新して、もし途中でコルーチンを止めた場合でも
        //正しいフィル値から計算し始める
        previousFillAmount = currentFillAmout;
        t = 0;

        while(t < 1f)
        {
            t += Time.deltaTime * fillSpeed;
            //lerp使て線形補間させる
            currentFillAmout = Mathf.Lerp(previousFillAmount, targetFillAmout, t);
            image.fillAmount = currentFillAmout;

            yield return null;
        }
        
    }

}
