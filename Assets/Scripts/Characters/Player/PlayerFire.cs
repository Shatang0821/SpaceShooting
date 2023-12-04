using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [Header("---- FIRE ----")]
    [Tooltip("�L�����N�^�[�̒e�I�u�W�F�N�g�ł��B")]
    [SerializeField] GameObject projectile1; //�e�I�u�W�F�N�g
    [SerializeField] GameObject projectile2; //�e�I�u�W�F�N�g
    [SerializeField] GameObject projectile3; //�e�I�u�W�F�N�g
    [SerializeField] GameObject projectileOverdrive;

    [Tooltip("�L�����N�^�[�̒e���ˈʒu�ł��B")]
    [SerializeField] Transform muzzleMiddle;        //�e���ˈʒu
    [SerializeField] Transform muzzleTop;           //�e���ˈʒu
    [SerializeField] Transform muzzleBottom;        //�e���ˈʒu

    [Tooltip("�L�����N�^�[�̒e���ˉ��ł��B")]
    [SerializeField] AudioData projectileLaunchSFX;   //���ˌ��ʉ�

    [Tooltip("�L�����N�^�[�̃p���[�ł��B")]
    [SerializeField, Range(0, 2)] int weaponPower = 0;

    [Tooltip("�L�����N�^�[�̒e���ˊԊu�ł��B")]
    [SerializeField] float fireInterval = 0.2f;         //�e���ˊԊu
    [SerializeField] float overdriveFireFactor = 1.2f;  //�U���Ԋu1.2�{�k��

    WaitForSeconds waitForOverdriveFireInterval;//�I�[�o�[�h���C�u�̍U���Ԋu

    WaitForSeconds waitForFireInterval;//�U���Ԋu

    MissileSystem missile;

    Player player;

    bool isOverdriving = false;

    void Initialized()
    {
        waitForFireInterval = new WaitForSeconds(fireInterval);

        player = GetComponent<Player>();

        missile = GetComponent<MissileSystem>();

        waitForOverdriveFireInterval = new WaitForSeconds(fireInterval /= overdriveFireFactor);
    }

    private void Awake()
    {
       Initialized();
    }

    private void OnEnable()
    {
        EventCenter.Subscribe(EventNames.Fire, Fire);

        EventCenter.Subscribe(EventNames.StopFire, StopFire);

        EventCenter.Subscribe(EventNames.LaunchMissile, LaunchMissile);

        EventCenter.Subscribe(EventNames.PlayerOverDriveOn, OverDriveOn);

        EventCenter.Subscribe(EventNames.OverDriveOff, OverDriveOff);

    }

    private void OnDisable()
    {
        EventCenter.Unsubscribe(EventNames.Fire, Fire);

        EventCenter.Unsubscribe(EventNames.StopFire, StopFire);

        EventCenter.Unsubscribe(EventNames.LaunchMissile, LaunchMissile);

        EventCenter.Unsubscribe(EventNames.PlayerOverDriveOn, OverDriveOn);

        EventCenter.Unsubscribe(EventNames.OverDriveOff, OverDriveOff);
    }

    #region FIRE

    //�C�x���g�����s�����炱����Ăяo��
    void Fire()
    {
        StartCoroutine(nameof(FireCoroutine));
    }
    //�L�[�𗣂�����~�܂�
    void StopFire()
    {
        //StopCoroutine(FireCoroutine());//��p���Ȃ�
        StopCoroutine(nameof(FireCoroutine));
    }

    /// <summary>
    //   �U���R���[�`��
    /// </summary>
    /// <returns></returns>
    IEnumerator FireCoroutine()
    {
        while (true)
        {
            switch (weaponPower)
            {
                case 0:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleMiddle.position);//�e�𐶐�����
                    break;
                case 1:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleTop.position);//�e�𐶐�����
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleBottom.position);//�e�𐶐�����
                    break;
                case 2:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleMiddle.position);//�e�𐶐�����
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile2, muzzleTop.position);//�e�𐶐�����
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile3, muzzleBottom.position);//�e�𐶐�����
                    break;
                default:
                    break;

            }

            AudioManager.Instance.PlayRandomSFX(projectileLaunchSFX);
            //�I�[�o�[�h���C�u��ԂȂ炻�̍U�����x
            yield return isOverdriving ? waitForOverdriveFireInterval : waitForFireInterval;

        }
    }
    #endregion

    void LaunchMissile()
    {
        missile.Launch(muzzleMiddle);
    }

    void OverDriveOn()
    {
        isOverdriving = true;
    }

    void OverDriveOff()
    {
        isOverdriving = false;
    }
}
