using Event;
using EnemyManagment;
using UnityEngine;
using Assets.Scripts.Interface;
using Assets.Scripts.Characters.Enemies;
using System.Collections.Generic;

public class TEST_EnemyManager : Singleton<TEST_EnemyManager>
{
    private WaveManager _waveManager;
    private SpawnManager _spawnManager;

    private EnemyFactory _enemyFactory;
    private EnemyBehavior _enemyBehavior;

    private List<Enemy> _enemyList = new List<Enemy>();
    private List<Enemy> _enemiesToRemove = new List<Enemy>();

    private Dictionary<GameObject, Enemy> _enemyDictionary;
    protected override void Awake()
    {
        base.Awake();
        _enemyFactory = new EnemyFactory();
        _enemyDictionary = new Dictionary<GameObject, Enemy>();
    }
    // void  Awake()
    //{
    //    //_waveManager = new WaveManager();
    //    //_spawnManager = new SpawnManager();
    //    _enemyList = new List<Enemy>();
    //    _enemyFactory = new EnemyFactory();
    //}

    private void Start()
    {
        for(int i = 0; i< 10;i++)
        {
            var enemy = _enemyFactory.GetEnemy(AircraftType.Aircraf01);
            enemy.SetEnemyPos(new Vector3(10, i, 0));
            StartCoroutine(enemy.Behavior.Attack());

            _enemyList.Add(enemy);
            _enemyDictionary.Add(enemy.EnemyPrefab, enemy);
        }
        
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
            if (enemy.IsActive == false)
            {
                _enemiesToRemove.Add(enemy);
            }

        }

        Remove();
    }
    /// <summary>
    /// リストから不要なオブジェクトを削除
    /// </summary>
    void Remove()
    {
        
        foreach (var enemy in _enemiesToRemove)
        {
            _enemyList.Remove(enemy);
            _enemyDictionary.Remove(enemy.EnemyPrefab); // 敵をDictionaryからも削除
            enemy.ResetValues();
        }
        _enemiesToRemove.Clear();
    }

    public void Damage(GameObject gameObject, float damage)
    {
        var enemy = _enemyDictionary[gameObject];
        if(enemy.IsActive)
        {
            Debug.Log(enemy.Health);
            enemy.TakenDamage(damage);
        }
    }
}
