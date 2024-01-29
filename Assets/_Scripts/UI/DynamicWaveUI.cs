using System.Collections;
using UnityEngine;

public class DynamicWaveUI : MonoBehaviour
{
    #region FIELDS
    [SerializeField] float animationTime = 1f;

    [Header("---- LINE MOVE ----")]
    //LINEが左右から中央に動くからスタート位置と目標位置を設定する
    [SerializeField] Vector2 lineTopStartPosition = new Vector2(-1250f, 140f);
    [SerializeField] Vector2 lineTopTargetPosition = new Vector2(0f, 140f);
    [SerializeField] Vector2 lineBottomStartPosition = new Vector2(1250f, 0f);
    [SerializeField] Vector2 lineBottomTargetPosition = Vector2.zero;


    [Header("---- TEXT SCALE ----")]
    //TEXTは縮小拡大エフェクトするためScale
    [SerializeField] Vector2 waveTextStartScale = new Vector2(1f, 0f);
    [SerializeField] Vector2 waveTextTargetScale = Vector2.one;

    RectTransform lineTop;      //上の線位置
    RectTransform lineBottom;   //下の線位置
    RectTransform waveText;     //テキスト位置

    WaitForSeconds waitStayTime;
    #endregion

    #region UNITY EVENT FUNCTIONS
    void Awake()
    {
        //Animatorで制御する場合これを削除
        if (TryGetComponent<Animator>(out Animator animator))
        {
            if (animator.isActiveAndEnabled)
            {
                Destroy(this);
            }
        }

        //生成間隔-スタートアニメーション時間とエンドアニメーション時間
        waitStayTime = new WaitForSeconds(EnemyManager.Instance.TimeBetweenWaves - animationTime * 2f);

        //初期化
        lineTop = transform.Find("Line Top").GetComponent<RectTransform>();
        lineBottom = transform.Find("Line Bottom").GetComponent<RectTransform>();
        waveText = transform.Find("Wave Text").GetComponent<RectTransform>();

        lineTop.localPosition = lineTopStartPosition;
        lineBottom.localPosition = lineBottomStartPosition;
        waveText.localScale = waveTextStartScale;
    }

    //コルーチンを開始させる
    void OnEnable()
    {
        StartCoroutine(LineMoveCoroutine(lineTop, lineTopTargetPosition, lineTopStartPosition));
        StartCoroutine(LineMoveCoroutine(lineBottom, lineBottomTargetPosition, lineBottomStartPosition));
        StartCoroutine(TextScaleCoroutine(waveText, waveTextTargetScale, waveTextStartScale));
    }
    #endregion

    #region LINE MOVE
    /// <summary>
    /// UIを移動させる
    /// </summary>
    /// <param name="rect">UIそれぞれの位置情報</param>
    /// <param name="targetPosition">それぞれの目標一</param>
    /// <param name="startPosition">スタート位置</param>
    /// <returns></returns>
    IEnumerator LineMoveCoroutine(RectTransform rect, Vector2 targetPosition, Vector2 startPosition)
    {
        yield return StartCoroutine(UIMoveCoroutine(rect, targetPosition));
        yield return waitStayTime;
        yield return StartCoroutine(UIMoveCoroutine(rect, startPosition));
    }

    /// <summary>
    /// 実際の移動処理
    /// </summary>
    /// <param name="rect">移動するUI</param>
    /// <param name="position">移動先</param>
    /// <returns></returns>
    IEnumerator UIMoveCoroutine(RectTransform rect, Vector2 position)
    {
        float t = 0f;
        Vector2 localPosition = rect.localPosition;

        while (t < 1f)
        {
            t += Time.deltaTime / animationTime;
            rect.localPosition = Vector2.Lerp(localPosition, position, t);

            yield return null;
        }
    }
    #endregion

    #region TEXT SCALE
    IEnumerator TextScaleCoroutine(RectTransform rect, Vector2 targetScale, Vector2 startScale)
    {
        yield return StartCoroutine(UIScaleCoroutine(rect, targetScale));
        yield return waitStayTime;
        yield return StartCoroutine(UIScaleCoroutine(rect, startScale));
    }

    IEnumerator UIScaleCoroutine(RectTransform rect, Vector2 scale)
    {
        float t = 0f;
        Vector2 localScale = rect.localScale;

        while (t < 1f)
        {
            t += Time.deltaTime / animationTime;
            rect.localScale = Vector2.Lerp(localScale, scale, t);

            yield return null;
        }
    }
    #endregion
}
