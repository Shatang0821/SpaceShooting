using Event;
using EnemyManagment;
using UnityEngine;
using Assets.Scripts.Interface;
using Assets.Scripts.Characters.Enemies;
using System.Collections.Generic;

public class TEST_EnemyManager : MonoBehaviour
{
    private WaveManager _waveManager;
    private SpawnManager _spawnManager;

    private EnemyFactory _enemyFactory;
    private EnemyBehavior _enemyBehavior;

    private List<Enemy> _enemyList;
    private void Awake()
    {
        //_waveManager = new WaveManager();
        //_spawnManager = new SpawnManager();
        _enemyList = new List<Enemy>();
        _enemyFactory = new EnemyFactory();
    }

    private void Start()
    {
        var enemy = _enemyFactory.GetEnemy(AircraftType.Aircraf01);
        enemy.SetEnemyPos(new Vector3(10,0,0));
        _enemyList.Add(enemy);

        StartCoroutine(enemy.Behavior.Attack());
        //_waveManager.Initialize();
    }

    private void Update()
    {
        //switch (_waveManager.CurrentState)
        //{
        //    case WaveState.Waiting:
        //        break;
        //    case WaveState.InProgress:
        //        break;
        //    case WaveState.Completed: 
        //        break;
        //}
        foreach (var enemy in _enemyList)
        {
            enemy.Update();
        }
    }
}
