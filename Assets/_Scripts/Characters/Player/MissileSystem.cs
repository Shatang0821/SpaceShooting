using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSystem : MonoBehaviour
{
    [SerializeField] int defaultAmount = 5;     //�f�t�H���g����
    [SerializeField] float cooldownTime = 1f;   //�N�[���_�E������

    [SerializeField] GameObject missilePrefab = null;   //�~�T�C���̃v���n�u

    [SerializeField] AudioData launchSFX = null;        //���ˌ��ʉ�
    [SerializeField] AudioData NoneAmount = null;       //�Ȃ��Ȃ������ʉ�

    bool isReady = true;    //�N�[���_�E���I���`�F�b�N

    int amount;             //�~�T�C���ʂ�ۑ�����

    private void Awake()
    {
        amount = defaultAmount;

    }

    private void Start()
    {
        MissileDisplay.UpdateAmountText(amount);
    }
    
    /// <summary>
    /// ���˂�����
    /// </summary>
    /// <param name="muzzleTransform">���ˌ����W���</param>
    public void Launch(Transform muzzleTransform)
    {
        //����0�܂��̓N�[���_�E�������˂������Ȃ�
        if (amount == 0 || !isReady)
        {
            AudioManager.Instance.PlaySFX(NoneAmount);
            MissileDisplay.ShakeIcon();
            return;
        }
        //�����łȂ����
        isReady = false;
        //�~�T�C���𐶐�
        PoolManager.Release(missilePrefab, muzzleTransform.position);
        //���𗬂�
        AudioManager.Instance.PlayRandomSFX(launchSFX);
        //��������
        amount--;
        //�e�L�X�g�A�b�v�f�[�g
        MissileDisplay.UpdateAmountText(amount);

        //����0�ɂȂ�����
        if(amount == 0)
        {
            //fill�l���P�ɂ���
            MissileDisplay.UpdateCooldownImage(1f);
        }
        else
        {
            //�N�[���_�E���J�n
            StartCoroutine(CooldownCoroutine());
        }
    }

    /// <summary>
    /// �N�[���_�E��
    /// </summary>
    /// <returns></returns>
    IEnumerator CooldownCoroutine()
    {

        var cooldownValue = cooldownTime;
        //�N�[���_�E����0�ɂȂ�܂ő���
        while (cooldownValue > 0f)
        {
            //�N�[���_�E��Fill�l�ω�
            MissileDisplay.UpdateCooldownImage(cooldownValue / cooldownTime);
            //�P�b���ƂɌ���ŏ�0�ȉ��ɂȂ�Ȃ�
            cooldownValue = Mathf.Max(cooldownValue - Time.deltaTime,0f);

            yield return null;
        }
        //�N�[���_�E���I��������甭�ˉ\������
        isReady = true;
    }
}
