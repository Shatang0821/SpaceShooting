using Assets.Scripts.EventCenter;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [Header("---- FIRE ----")]
    [Tooltip("キャラクターの弾オブジェクトです。")]
    [SerializeField] GameObject projectile1; //弾オブジェクト
    [SerializeField] GameObject projectile2; //弾オブジェクト
    [SerializeField] GameObject projectile3; //弾オブジェクト
    [SerializeField] GameObject projectileOverdrive;

    [Tooltip("キャラクターの弾発射位置です。")]
    [SerializeField] Transform muzzleMiddle;        //弾発射位置
    [SerializeField] Transform muzzleTop;           //弾発射位置
    [SerializeField] Transform muzzleBottom;        //弾発射位置

    [Tooltip("キャラクターの弾発射音です。")]
    [SerializeField] AudioData projectileLaunchSFX;   //発射効果音

    [Tooltip("キャラクターのパワーです。")]
    [SerializeField, Range(0, 2)] int weaponPower = 0;

    [Tooltip("キャラクターの弾発射間隔です。")]
    [SerializeField] float fireInterval = 0.2f;         //弾発射間隔
    [SerializeField] float overdriveFireFactor = 1.2f;  //攻撃間隔1.2倍縮む

    WaitForSeconds waitForOverdriveFireInterval;//オーバードライブの攻撃間隔

    WaitForSeconds waitForFireInterval;//攻撃間隔

    MissileSystem missile;

    Player player;

    bool isOverdriving = false;

    [SerializeField] protected bool CanLaunchMissile = true;

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

    protected virtual void OnEnable()
    {
        EventCenter.Subscribe(EventKeyManager.Fire, Fire);

        EventCenter.Subscribe(EventKeyManager.StopFire, StopFire);

        EventCenter.Subscribe(EventKeyManager.LaunchMissile, LaunchMissile);

        EventCenter.Subscribe(EventKeyManager.PlayerOverDriveOn, OverDriveOn);

        EventCenter.Subscribe(EventKeyManager.OverDriveOff, OverDriveOff);

    }

    protected virtual void OnDisable()
    {
        EventCenter.Unsubscribe(EventKeyManager.Fire, Fire);

        EventCenter.Unsubscribe(EventKeyManager.StopFire, StopFire);

        EventCenter.Unsubscribe(EventKeyManager.LaunchMissile, LaunchMissile);

        EventCenter.Unsubscribe(EventKeyManager.PlayerOverDriveOn, OverDriveOn);

        EventCenter.Unsubscribe(EventKeyManager.OverDriveOff, OverDriveOff);
    }

    #region FIRE

    //イベントを実行したらこれを呼び出す
    protected void Fire()
    {
        StartCoroutine(nameof(FireCoroutine));
    }
    //キーを離したら止まる
    protected void StopFire()
    {
        //StopCoroutine(FireCoroutine());//作用しない
        StopCoroutine(nameof(FireCoroutine));
    }

    /// <summary>
    //   攻撃コルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator FireCoroutine()
    {
        while (true)
        {
            switch (weaponPower)
            {
                case 0:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleMiddle.position);//弾を生成する
                    break;
                case 1:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleTop.position);//弾を生成する
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleBottom.position);//弾を生成する
                    break;
                case 2:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleMiddle.position);//弾を生成する
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile2, muzzleTop.position);//弾を生成する
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile3, muzzleBottom.position);//弾を生成する
                    break;
                default:
                    break;

            }

            AudioManager.Instance.PlayRandomSFX(projectileLaunchSFX);
            //オーバードライブ状態ならその攻撃速度
            yield return isOverdriving ? waitForOverdriveFireInterval : waitForFireInterval;

        }
    }
    #endregion

    void LaunchMissile()
    {
        if (CanLaunchMissile)
        {
            missile.Launch(muzzleMiddle);
        }
    }

    protected void OverDriveOn()
    {
        isOverdriving = true;
    }

    protected void OverDriveOff()
    {
        isOverdriving = false;
    }
}
