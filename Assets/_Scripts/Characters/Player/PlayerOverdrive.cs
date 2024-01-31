using Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerOverdrive : MonoBehaviour
{
    //�J�n�G�t�F�N�g
    [SerializeField] GameObject triggerVFX;
    //���ʂ̃G���W���G�t�F�N�g��ێ����邽�߂̕ϐ�
    [SerializeField] GameObject engineVFXNormal;
    //�����Ԃɓ���G���W���G�t�F�N�g
    [SerializeField] GameObject engineVFXOverdrive;
    //�J�n���ʉ�
    [SerializeField] AudioData onSFX;
    //�I�����ʉ�
    [SerializeField] AudioData offSFX;

    private void Awake()
    {
        //�C�x���g�̃T�u�X�N���C�u
        EventCenter.Subscribe(EventKeyManager.InputOverDriveOn, Overdrive);

        EventCenter.Subscribe(EventKeyManager.PlayerOverDriveOn, On);
        EventCenter.Subscribe(EventKeyManager.OverDriveOff, Off);
    }

    private void OnDestroy()
    {
        //�C�x���g�̃A���T�u�X�N���C�u
        EventCenter.Unsubscribe(EventKeyManager.InputOverDriveOn, Overdrive);

        EventCenter.Unsubscribe(EventKeyManager.PlayerOverDriveOn, On);
        EventCenter.Unsubscribe(EventKeyManager.OverDriveOff, Off);
    }

    void Overdrive()
    {
        //�G�l���M�[������Ȃ��ꍇ���������Ȃ�
        if (!PlayerEnergy.Instance.IsEnough(PlayerEnergy.MAX)) return;

        EventCenter.TriggerEvent(EventKeyManager.PlayerOverDriveOn);
    }

    void On()
    {
        /*
            �G�t�F�N�g��؂�ւ���
         */
        triggerVFX.SetActive(true);
        engineVFXNormal.SetActive(false);
        engineVFXOverdrive.SetActive(true);
        AudioManager.Instance.PlayRandomSFX(onSFX);
    }

    void Off()
    {
        /*
            �G�t�F�N�g��߂�
         */
        engineVFXOverdrive.SetActive(false);
        engineVFXNormal.SetActive(true);
        AudioManager.Instance.PlayRandomSFX(offSFX);
    }
}
