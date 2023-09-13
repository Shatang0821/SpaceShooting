using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : Singleton<PlayerEnergy>
{

    [SerializeField] EnergyBar energyBar;

    //エネルギー最大値
    public const int MAX = 100;

    //パーセント
    public const int PERCENT = 1;

    int energy;

    private void Start()
    {
        energyBar.Initialize(energy, MAX);
    }

    //エネルギーチャージ
    public void Obtain(int value)
    {
        if (value == MAX) return;

        //エネルギーの範囲を制限する
        energy = Mathf.Clamp(energy += value, 0, MAX);
        energyBar.UpdateStats(energy, MAX);
    }

    //エネルギー消耗
    public void Use(int value)
    {
        energy -= value;
        energyBar.UpdateStats(energy, MAX);
    }

    //消耗しようとする量が持っているかをチェック
    public bool IsEnough(int value) => energy >= value;
}
