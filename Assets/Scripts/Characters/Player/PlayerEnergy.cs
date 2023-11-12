using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : Singleton<PlayerEnergy>
{
    
    [SerializeField] EnergyBar energyBar;

    [SerializeField] float overdriveInterval = 0.1f;//エネルギーが消耗間隔

    //オーバードライブでエネルギーの回復を制御する
    bool available = true;

    //エネルギー最大値
    public const int MAX = 100;

    //パーセント
    public const int PERCENT = 1;
    //現在エネルギー
    int energy;

    //消耗間隔
    WaitForSeconds waitForOverdriveInterval;

    protected override void Awake()
    {
        base.Awake();
        waitForOverdriveInterval = new WaitForSeconds(overdriveInterval);
    }
    private void OnEnable()
    {
        PlayerOverdrive.on += PlayerOverdriveOn;
        PlayerOverdrive.off += PlayerOverdriveOff;
    }

    private void OnDisable()
    {
        PlayerOverdrive.on -= PlayerOverdriveOn;
        PlayerOverdrive.off -= PlayerOverdriveOff;
    }

    private void Start()
    {
        energyBar.Initialize(energy, MAX);
        //初期でMaxを追加する
        Obtain(MAX);
    }

    //エネルギーチャージ
    public void Obtain(int value)
    {
        if (energy == MAX || !available || !gameObject.activeSelf) return;

        //エネルギーの範囲を制限する
        energy = Mathf.Clamp(energy + value, 0, MAX);
        //ここで少しずつ増えていく
        energyBar.UpdateStats(energy, MAX);
    }

    //エネルギー消耗
    public void Use(int value)
    {
        energy -= value;
        energyBar.UpdateStats(energy, MAX);

        if (energy == 0 && !available)
        {
            PlayerOverdrive.off.Invoke();
        }
    }

    //消耗しようとする量が持っているかをチェック
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
    /// エネルギーが持続で消耗される
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
