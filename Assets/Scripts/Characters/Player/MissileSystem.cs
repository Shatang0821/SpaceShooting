using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSystem : MonoBehaviour
{
    [SerializeField] int defaultAmount = 5;     //デフォルト数量
    [SerializeField] float cooldownTime = 1f;   //クールダウン時間

    [SerializeField] GameObject missilePrefab = null;   //ミサイルのプレハブ

    [SerializeField] AudioData launchSFX = null;        //発射効果音
    [SerializeField] AudioData NoneAmount = null;       //なくなった効果音

    bool isReady = true;    //クールダウン終わりチェック

    int amount;             //ミサイル量を保存する

    private void Awake()
    {
        amount = defaultAmount;

    }

    private void Start()
    {
        MissileDisplay.UpdateAmountText(amount);
    }
    
    /// <summary>
    /// 発射させる
    /// </summary>
    /// <param name="muzzleTransform">発射口座標情報</param>
    public void Launch(Transform muzzleTransform)
    {
        //数が0またはクールダウン中発射をさせない
        if (amount == 0 || !isReady)
        {
            AudioManager.Instance.PlaySFX(NoneAmount);
            MissileDisplay.ShakeIcon();
            return;
        }
        //そうでなければ
        isReady = false;
        //ミサイルを生成
        PoolManager.Release(missilePrefab, muzzleTransform.position);
        //音を流す
        AudioManager.Instance.PlayRandomSFX(launchSFX);
        //数を減る
        amount--;
        //テキストアップデート
        MissileDisplay.UpdateAmountText(amount);

        //数が0になったら
        if(amount == 0)
        {
            //fill値を１にする
            MissileDisplay.UpdateCooldownImage(1f);
        }
        else
        {
            //クールダウン開始
            StartCoroutine(CooldownCoroutine());
        }
    }

    /// <summary>
    /// クールダウン
    /// </summary>
    /// <returns></returns>
    IEnumerator CooldownCoroutine()
    {

        var cooldownValue = cooldownTime;
        //クールダウンが0になるまで続く
        while (cooldownValue > 0f)
        {
            //クールダウンFill値変化
            MissileDisplay.UpdateCooldownImage(cooldownValue / cooldownTime);
            //１秒ごとに減る最小0以下にならない
            cooldownValue = Mathf.Max(cooldownValue - Time.deltaTime,0f);

            yield return null;
        }
        //クールダウン終わったから発射可能させる
        isReady = true;
    }
}
