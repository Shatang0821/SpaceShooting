using Event;
using EnemyManagment;
using UnityEngine;
using Assets.Scripts.Interface;
using Assets.Scripts.Characters.Enemies;
using System.Collections.Generic;
public enum EnemyManagerState
{
    Waiting,
    InProgress,
    Completed
}
public class TEST_EnemyManager : Singleton<TEST_EnemyManager>
{
    private WaveManager _waveManager;
    private SpawnManager _spawnManager;

    [SerializeField]
    private EnemyManagerState _currentState;

    private EnemyBehavior _enemyBehavior;

    private List<Enemy> _enemyList = new List<Enemy>();
    private List<Enemy> _enemiesToRemove = new List<Enemy>();

    private Dictionary<GameObject, Enemy> _enemyDictionary = new Dictionary<GameObject, Enemy>();
    protected override void Awake()
    {
        base.Awake();
    }

    private void Initialize()
    {
        _spawnManager = new SpawnManager();


        _waveManager = new WaveManager();
        _waveManager.Initialize(DataManager.Instance.EnemyWaveDatas);

        _currentState = EnemyManagerState.Waiting;
    }
    private void Start()
    {
        _enemyList = _spawnManager.GetEnemy(AircraftType.Aircraf01, 5);

        SetDictionary(_enemyList);
        //_waveManager.Initialize();
    }

    private void Update()
    {
        switch (_currentState)
        {
            case EnemyManagerState.Waiting:
                _currentState = EnemyManagerState.InProgress;
                break;
            case EnemyManagerState.InProgress:
                InProgressUpdate();
                break;
            case EnemyManagerState.Completed:

                break;
        }
    }

    private void InWaiting()
    {

    }

    private void InProgressUpdate()
    {
        if (_enemyList.Count > 0)
        {
            foreach (var enemy in _enemyList)
            {
                enemy.Update();
                if (enemy.IsActive == false)
                {
                    _enemiesToRemove.Add(enemy);
                }

            }
        }
        else
        {
            _currentState = EnemyManagerState.Completed;
        }
        if (_enemiesToRemove.Count > 0)
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
    /// <summary>
    /// 攻撃開始と辞書登録
    /// </summary>
    /// <param name="enemies"></param>
    public void SetDictionary(List<Enemy> enemies)
    {
        foreach(var enemy in enemies)
        {
            _enemyDictionary.Add(enemy.EnemyPrefab, enemy);
            StartCoroutine(enemy.Behavior.Attack());
        }
    }
}
