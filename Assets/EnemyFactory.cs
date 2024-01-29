using Assets._Scripts.Characters.Enemies;
using Assets._Scripts.Pool_System;
using System.Collections.Generic;
using UnityEngine;

public enum AircraftType
{
    Enemy01,
    Enemy02,
    Enemy03
    // ���̑��̋@�̃^�C�v
}

public class EnemyFactory
{
    private static EnemyFactory _instance;


    private const int SIZE = 50;                    //Enemy�C���X�^���X�T�C�Y

    private EnemyPool _enemyPool;                   //�G�l�~�[�C���X�^���X�v�[�����쐬

    private Dictionary<AircraftType, EnemyAircraft> _enemyAircraftDictionary;

    private EnemyAircraftData[] _enemyDatas;

    public EnemyFactory()
    {
        _enemyAircraftDictionary = new Dictionary<AircraftType, EnemyAircraft>();
        _enemyPool = new EnemyPool(SIZE);
        foreach(var enemyData in _enemyDatas)
        {
            var aircraft = new EnemyAircraft(enemyData.EnemyPrefab, enemyData.ProjectilePrefabs);
            _enemyAircraftDictionary.Add(enemyData.Type, aircraft);
        }
    }

    public static EnemyFactory Instance
    {
        get { 
            if (_instance == null)
            {
                _instance = new EnemyFactory();
            }
            return _instance; 
        }
    }

    /// <summary>
    /// �w��^�C�v�̃f�[�^��Ԃ�
    /// </summary>
    /// <param name="aircraftType">�@�̃^�C�v</param>
    /// <returns></returns>
    public EnemyAircraft GetEnemyData(AircraftType aircraftType)
    {
        return _enemyAircraftDictionary[aircraftType];
    }
}
