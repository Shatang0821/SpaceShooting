using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : Singleton<PlayerEnergy>
{
    
    [SerializeField] EnergyBar energyBar;

    [SerializeField] float overdriveInterval = 0.1f;//�G�l���M�[�����ՊԊu

    //�I�[�o�[�h���C�u�ŃG�l���M�[�̉񕜉\��������
    bool available = true;

    //�G�l���M�[�ő�l
    public const int MAX = 100;

    //�p�[�Z���g
    public const int PERCENT = 1;
    //���݃G�l���M�[
    int energy;

    //���ՊԊu
    WaitForSeconds waitForOverdriveInterval;

    protected override void Awake()
    {
        base.Awake();
        waitForOverdriveInterval = new WaitForSeconds(overdriveInterval);
    }
    private void OnEnable()
    {
        EventCenter.Subscribe(EventNames.PlayerOverDriveOn, PlayerOverdriveOn);
        EventCenter.Subscribe(EventNames.OverDriveOff, PlayerOverdriveOff);
    }

    private void OnDisable()
    {
        EventCenter.Unsubscribe(EventNames.PlayerOverDriveOn, PlayerOverdriveOn);
        EventCenter.Unsubscribe(EventNames.OverDriveOff, PlayerOverdriveOff);
    }

    private void Start()
    {
        energyBar.Initialize(energy, MAX);
        //������Max��ǉ�����
        Obtain(MAX);
    }

    //�G�l���M�[�`���[�W
    public void Obtain(int value)
    {
        if (energy == MAX || !available || !gameObject.activeSelf) return;

        //�G�l���M�[�͈̔͂𐧌�����
        energy = Mathf.Clamp(energy + value, 0, MAX);
        //�����ŏ����������Ă���
        energyBar.UpdateStats(energy, MAX);
    }

    //�G�l���M�[����
    public void Use(int value)
    {
        energy -= value;
        energyBar.UpdateStats(energy, MAX);

        if (energy == 0 && !available)
        {
            EventCenter.TriggerEvent(EventNames.OverDriveOff);
        }
    }

    //���Ղ��悤�Ƃ���ʂ������Ă��邩���`�F�b�N
    public bool IsEnough(int value) => energy >= value;

    void PlayerOverdriveOn()
    {
        available = false;
        StartCoroutine(nameof(KeepUsingCoroutine));
    }

    void PlayerOverdriveOff()
    {
        available = true;
        StopCoroutine(nameof(KeepUsingCoroutine));
    }

    /// <summary>
    /// �G�l���M�[�������ŏ��Ղ����
    /// </summary>
    /// <returns></returns>
    IEnumerator KeepUsingCoroutine()
    {
        while (gameObject.activeSelf && energy > 0)
        {
            yield return waitForOverdriveInterval;

            //0.1s 1% 1S 10% 10S 100%
            Use(PERCENT * 2);
        }
    }
}
