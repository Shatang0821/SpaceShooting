using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerOverdrive : MonoBehaviour
{
    //開始エフェクト
    [SerializeField] GameObject triggerVFX;
    //普通のエンジンエフェクトを保持するための変数
    [SerializeField] GameObject engineVFXNormal;
    //特殊状態に入るエンジンエフェクト
    [SerializeField] GameObject engineVFXOverdrive;
    //開始効果音
    [SerializeField] AudioData onSFX;
    //終了効果音
    [SerializeField] AudioData offSFX;

    private void Awake()
    {
        //イベントのサブスクライブ
        EventCenter.Subscribe(EventNames.InputOverDriveOn, Overdrive);

        EventCenter.Subscribe(EventNames.PlayerOverDriveOn, On);
        EventCenter.Subscribe(EventNames.OverDriveOff, Off);
    }

    private void OnDestroy()
    {
        //イベントのアンサブスクライブ
        EventCenter.Unsubscribe(EventNames.InputOverDriveOn, Overdrive);

        EventCenter.Unsubscribe(EventNames.PlayerOverDriveOn, On);
        EventCenter.Unsubscribe(EventNames.OverDriveOff, Off);
    }

    void Overdrive()
    {
        //エネルギーが足りない場合処理させない
        if (!PlayerEnergy.Instance.IsEnough(PlayerEnergy.MAX)) return;

        EventCenter.TriggerEvent(EventNames.PlayerOverDriveOn);
    }

    void On()
    {
        /*
            エフェクトを切り替える
         */
        triggerVFX.SetActive(true);
        engineVFXNormal.SetActive(false);
        engineVFXOverdrive.SetActive(true);
        AudioManager.Instance.PlayRandomSFX(onSFX);
    }

    void Off()
    {
        /*
            エフェクトを戻す
         */
        engineVFXOverdrive.SetActive(false);
        engineVFXNormal.SetActive(true);
        AudioManager.Instance.PlayRandomSFX(offSFX);
    }
}
