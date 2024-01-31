using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : PersistentSingleton<DataManager> 
{
    [SerializeField]
    private EnemyAircraftData[] _enemyAircraftDatas;
    public EnemyAircraftData[] EnemyAircraftDatas { get => _enemyAircraftDatas; private set => _enemyAircraftDatas = value; }

    [SerializeField]
    private EnemyWaveData[] _enemyWaveDatas;
    public EnemyWaveData[] EnemyWaveDatas { get => _enemyWaveDatas; private set => _enemyWaveDatas = value;}
}
