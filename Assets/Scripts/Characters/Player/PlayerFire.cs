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

    //イベントを実行したらこれを呼び出す
    void Fire()
    {
        StartCoroutine(nameof(FireCoroutine));
    }
    //キーを離したら止まる
    void StopFire()
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
