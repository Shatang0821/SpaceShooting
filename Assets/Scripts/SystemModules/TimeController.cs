using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : Singleton<TimeController>
{
    [SerializeField,Range(0,1f)] float bulletTimeScale = 0.1f;  //バレットタイム

    float defaultFixedDeltaTime;                                //元のFixedDeltaTime;

    float timeScaleBeforePause;

    float t;                                                    //lerpの第三引数
    protected override void Awake()
    {
        base.Awake();
        defaultFixedDeltaTime = Time.fixedDeltaTime;
    }

    /// <summary>
    /// timeScaleを0にする
    /// </summary>
    public void Pause()
    {
        timeScaleBeforePause = Time.timeScale;
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Pause開始時のtimeScaleに戻る
    /// </summary>
    public void UnPause()
    {
        Time.timeScale = timeScaleBeforePause;
    }

    #region BULLET TIME
    /// <summary>
    /// バレットタイムスタートさせる
    /// </summary>
    /// <param name="duration">時間長さ</param>
    public void BulletTime(float duration)
    {
        Time.timeScale = bulletTimeScale;
        StartCoroutine(SlowOutCoroutine(duration));
    }

    /// <summary>
    /// in out両方ある
    /// </summary>
    /// <param name="inDuration">In持続時間</param>
    /// <param name="outDuration">Out持続時間</param>
    public void BulletTime(float inDuration,float outDuration)
    {
        StartCoroutine(SlowInAndOutCoroutine(inDuration,outDuration));
    }

    public void BulletTime(float inDuration, float keepingDuration, float outDuration)
    {
        StartCoroutine(SlowInKeepAndOutDuration(inDuration,keepingDuration,outDuration));
    }

    public void SlowIn(float duration)
    {
        StartCoroutine(SlowInCoroutine(duration));
    }

    public void SlowOut(float duration)
    {
        StartCoroutine(SlowOutCoroutine(duration));
    }

    IEnumerator SlowInKeepAndOutDuration(float inDuration,float keepingDuration, float outDuration)
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));

        yield return new WaitForSecondsRealtime(keepingDuration);

        StartCoroutine(SlowOutCoroutine(outDuration));
    }

    /// <summary>
    /// バレットタイムスタート
    /// </summary>
    /// <param name="inDuration">In持続時間</param>
    /// <param name="outDuration">Out持続時間</param>
    /// <returns></returns>
    IEnumerator SlowInAndOutCoroutine(float inDuration,float outDuration)
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));

        StartCoroutine(SlowOutCoroutine(outDuration));
    }


    /// <summary>
    /// ゆっくり始める
    /// </summary>
    /// <param name="duration">持続時間</param>
    /// <returns></returns>
    IEnumerator SlowInCoroutine(float duration)
    {
        t = 0f;

        while (t < 1f)
        {
            if(GameManager.GameState != GameState.Paused )
            {
                //Time.deltaTimeは timescale変わるときも変わるから使えない
                t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(1f, bulletTimeScale, t);
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
            }
            yield return null;
        }
    }

    /// <summary>
    /// バレットタイムをゆっくり終わらせる
    /// </summary>
    /// <param name="duration">時間長さ</param>
    /// <returns></returns>
    IEnumerator SlowOutCoroutine(float duration)
    {
        t = 0f;

        while(t < 1f)
        {
            if(GameManager.GameState != GameState.Paused)
            {
                //Time.deltaTimeは timescale変わるときも変わるから使えない
                t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(bulletTimeScale, 1f, t);
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
            }
            yield return null;
        }
    }
    #endregion
}
