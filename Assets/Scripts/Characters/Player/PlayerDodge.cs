using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodge : MonoBehaviour
{
    #region PlayerDodge
    [Header("----- DODGE -----")]

    [SerializeField] AudioData dodgeSFX;//効果音

    [Tooltip("これはキャラクターのエネルギー消耗量です。")]
    [SerializeField, Range(0, 100)] int dodgeEnergyCost = 25;

    [SerializeField] float maxRoll = 720f;//最大回転角度

    [SerializeField] float rollSpeed = 360f;//回転速度
    [SerializeField] Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);//スケール変化

    bool isDodging = false; //ダッジ　躱すかわす

    float currentRoll;//現在回転値

    float dodgeDuration;//ダッジ持続時間

    readonly float slowMotionDuration = 1f;//バレットタイムslowout時間
    #endregion

    new Collider2D collider;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();

        dodgeDuration = maxRoll / rollSpeed;//最大回転角度/回転速度 = 時間
    }

    private void OnEnable()
    {
        EventCenter.Subscribe(EventNames.Dodge, Dodge);
    }

    private void OnDisable()
    {
        EventCenter.Unsubscribe(EventNames.Dodge, Dodge);
    }
    #region DODGE
    void Dodge()
    {
        //もしダッジ中であれば処理させないまた、エネルギーが足りないときも同様
        if (isDodging || !PlayerEnergy.Instance.IsEnough(dodgeEnergyCost)) return;
        //そうでなければ処理に入る
        StartCoroutine(nameof(DodgeCoroutine));
        // Change Player's scale
    }

    /// <summary>
    /// ダッジ実行コルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator DodgeCoroutine()
    {
        isDodging = true;
        AudioManager.Instance.PlayRandomSFX(dodgeSFX);
        // エネルギーを消耗
        PlayerEnergy.Instance.Use(dodgeEnergyCost);

        //物体と通過できるようにする
        collider.isTrigger = true;

        // 初期回転を0にする
        currentRoll = 0f;

        //バレットタイムを開始
        TimeController.Instance.BulletTime(slowMotionDuration, slowMotionDuration);

        //現在回転が最大回転より小さい場合ループさせる
        while (currentRoll < maxRoll)
        {
            currentRoll += rollSpeed * Time.deltaTime;
            //回転させる
            transform.rotation = Quaternion.AngleAxis(currentRoll, Vector3.right);

            //ベジェ曲線を使って動きをなめらかにする
            //scaleを1から0.5また1に戻るようにする
            transform.localScale = BezierCurve.QuadraticPoint(Vector3.one, Vector3.one, dodgeScale, currentRoll / maxRoll);
            yield return null;
        }

        collider.isTrigger = false;
        isDodging = false;
    }
    #endregion

}
