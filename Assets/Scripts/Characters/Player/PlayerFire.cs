using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [Header("---- FIRE ----")]
    [Tooltip("これはキャラクターの弾オブジェクトです。")]
    [SerializeField] GameObject projectile1; //弾オブジェクト
    [SerializeField] GameObject projectile2; //弾オブジェクト
    [SerializeField] GameObject projectile3; //弾オブジェクト
    [SerializeField] GameObject projectileOverdrive;

    [Tooltip("これはキャラクターの弾発射位置です。")]
    [SerializeField] Transform muzzleMiddle;      //弾発射位置
    [SerializeField] Transform muzzleTop;      //弾発射位置
    [SerializeField] Transform muzzleBottom;      //弾発射位置

    [SerializeField] AudioData projectileLaunchSFX;   //発射効果音

    [Tooltip("これはキャラクターのパワーです。")]
    [SerializeField, Range(0, 2)] int weaponPower = 0;

    [Tooltip("これはキャラクターの弾発射間隔です。")]
    [SerializeField] float fireInterval = 0.2f;    //弾発射間隔

    WaitForSeconds waitForFireInterval;//攻撃間隔

    MissileSystem missile;

    Player player;

    bool isOverdriving = false;

    private void Awake()
    {
        waitForFireInterval = new WaitForSeconds(fireInterval);

        player = GetComponent<Player>();

        missile = GetComponent<MissileSystem>();
    }

    private void OnEnable()
    {
        EventCenter.Subscribe(EventNames.Fire, Fire);

        EventCenter.Subscribe(EventNames.StopFire, StopFire);

        EventCenter.Subscribe(EventNames.LaunchMissile, LaunchMissile);
    }

    private void OnDisable()
    {
        EventCenter.Unsubscribe(EventNames.Fire, Fire);

        EventCenter.Unsubscribe(EventNames.StopFire, StopFire);

        EventCenter.Unsubscribe(EventNames.LaunchMissile, LaunchMissile);
    }

    #region FIRE

    //イベントを実行したらこれを呼び出す
    void Fire()
    {
        StartCoroutine(nameof(FireCoroutine));
    }
    //キーを話したら止まる
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
            yield return isOverdriving ? player.waitForOverdriveFireInterval : waitForFireInterval;

        }
    }
    #endregion

    void LaunchMissile()
    {
        missile.Launch(muzzleMiddle);
    }
}
