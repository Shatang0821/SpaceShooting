using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : Singleton<PlayerEnergy>
{

    [SerializeField] EnergyBar energyBar;

    //�G�l���M�[�ő�l
    public const int MAX = 100;

    //�p�[�Z���g
    public const int PERCENT = 1;

    int energy;

    private void Start()
    {
        energyBar.Initialize(energy, MAX);
    }

    //�G�l���M�[�`���[�W
    public void Obtain(int value)
    {
        if (value == MAX) return;

        //�G�l���M�[�͈̔͂𐧌�����
        energy = Mathf.Clamp(energy += value, 0, MAX);
        energyBar.UpdateStats(energy, MAX);
    }

    //�G�l���M�[����
    public void Use(int value)
    {
        energy -= value;
        energyBar.UpdateStats(energy, MAX);
    }

    //���Ղ��悤�Ƃ���ʂ������Ă��邩���`�F�b�N
    public bool IsEnough(int value) => energy >= value;
}
