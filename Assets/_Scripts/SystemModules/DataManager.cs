using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : PersistentSingleton<DataManager> 
{
    [SerializeField]
    private EnemyData[] _enemyDatas;
    public EnemyData[] EnemyDatas { get => _enemyDatas; private set => _enemyDatas = value; }

    [SerializeField]
    private EnemyWaveData[] _enemyWaveDatas;
    public EnemyWaveData[] EnemyWaveDatas { get => _enemyWaveDatas; private set => _enemyWaveDatas = value;}
}
