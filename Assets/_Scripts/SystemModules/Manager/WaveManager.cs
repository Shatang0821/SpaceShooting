using Assets.Scripts.Characters.Enemies;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : Singleton<WaveManager>
{
    [SerializeField]
    private Transform[] _spawnPos;
    public Wave[] Wave { get; private set; }
    //private int _currentWaveIndex = 0;
    public GameObject _enemyPrefab;

    private const int totalSpawnNumber = 1;
    private void Start()
    {
        Wave = new Wave[totalSpawnNumber];
        InitializeWaves();
    }
    private void InitializeWaves()
    {
        Wave[0] = new Wave
        {
            spawns = new List<SpawnData>
            {
                new SpawnData(_enemyPrefab,_spawnPos[0].position,0.2f,3),
                new SpawnData(_enemyPrefab,_spawnPos[1].position,0.2f,3),
                new SpawnData(_enemyPrefab,_spawnPos[2].position,0.2f,3)
            }
        };
    }
}

[System.Serializable]
public class Wave
{
    public List<SpawnData> spawns; // このウェーブでの敵の出現データ
}
