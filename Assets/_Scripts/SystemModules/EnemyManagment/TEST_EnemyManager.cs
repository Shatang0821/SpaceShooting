using Event;
using EnemyManagment;
using UnityEngine;
using Assets.Scripts.Interface;
using Assets.Scripts.Characters.Enemies;
using System.Collections.Generic;
using System.Collections;
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

    private List<Enemy> _enemyList = new List<Enemy>();

    private List<Enemy> _enemiesToRemove = new List<Enemy>();

    private Dictionary<GameObject, Enemy> _enemyDictionary = new Dictionary<GameObject, Enemy>();
    protected override void Awake()
    {
        base.Awake();

        Initialize();
    }

    private void Initialize()
    {
        _spawnManager = new SpawnManager();
        _waveManager = new WaveManager();

        _waveManager.Initialize(DataManager.Instance.EnemyWaveDatas);

        _currentState = EnemyManagerState.Waiting;
    }

    private IEnumerator Start()
    {
        while (true)
        {
            switch (_currentState)
            {
                case EnemyManagerState.Waiting:
                    yield return StartCoroutine(nameof(InWaiting));
                    break;
                case EnemyManagerState.InProgress:
                    yield return StartCoroutine(nameof(InProgressUpdate));
                    break;
                case EnemyManagerState.Completed:
                    yield return StartCoroutine(nameof(InCompleted));
                    break;
            }
            yield return null;
        }

    }

    /// <summary>
    /// Waveの設定
    /// </summary>
    /// <returns></returns>
    private IEnumerator InWaiting()
    {
        StartWave();

        yield return null;

    }

    /// <summary>
    /// waveの準備
    /// </summary>
    private void StartWave()
    {
        if(_waveManager.StartNextWave())
        {
            var wave = _waveManager.GetSpawnWave();

            _enemyList = _spawnManager.PrepareEnemies(wave);
            SetDictionary(_enemyList);

            _currentState = EnemyManagerState.InProgress;
        }
        else
        {
            _currentState = EnemyManagerState.Completed;
        }

        //Debug.Log("EnemyListの数は : " + _enemyList.Count);
    }

    /// <summary>
    /// 敵実行更新
    /// </summary>
    /// <returns></returns>
    private IEnumerator InProgressUpdate()
    {
        StartCoroutine(_spawnManager.Spawn(_enemyList));
        while (IsProgressUpdate)
        {
            foreach (var enemy in _enemyList)
            {
                if (enemy.IsActive)
                {
                    enemy.Update();
                }

                if(enemy.IsDied)
                {
                    if (!_enemiesToRemove.Contains(enemy))
                    {
                        //Debug.Log("Add");
                        _enemiesToRemove.Add(enemy);
                    }
                        
                }
                    
            }

            if (_enemiesToRemove.Count > 0 && !_spawnManager.IsSpawning)
            {
                Remove();
            }
                         
            yield return null;
        }
        _waveManager.UpdateIndex();
        _currentState = EnemyManagerState.Waiting;
        yield return null;
    }

    /// <summary>
    /// ゲーム終了
    /// </summary>
    /// <returns></returns>
    private IEnumerator InCompleted()
    {
        Debug.Log("ゲームクリア");
        yield return null;
    }

    private bool IsProgressUpdate => _enemyList.Count > 0 && GameManager.GameState != GameState.GameOver;

    /// <summary>
    /// リストから不要なオブジェクトを削除
    /// </summary>
    void Remove()
    {

        foreach (var enemy in _enemiesToRemove)
        {
            _enemyList.Remove(enemy);
            if(enemy.EnemyPrefab == null)
            {
                Debug.Log("ERROR");
            }
            else
            {
                Debug.Log("Delete From Dictionary");
                _enemyDictionary.Remove(enemy.EnemyPrefab); // 敵をDictionaryからも削除
            }
            
            
            enemy.ResetValues();
        }
        _enemiesToRemove.Clear();


    }

    public void Damage(GameObject gameObject, float damage)
    {
        var enemy = _enemyDictionary[gameObject];
        if (enemy.IsActive)
        {
            //Debug.Log(enemy.Health);
            enemy.TakenDamage(damage);
        }
    }
    /// <summary>
    /// 攻撃開始と辞書登録
    /// </summary>
    /// <param name="enemies"></param>
    public void SetDictionary(List<Enemy> enemies)
    {
        foreach (var enemy in enemies)
        {
            _enemyDictionary.Add(enemy.EnemyPrefab, enemy);
            StartCoroutine(enemy.Behavior.Attack());
        }
    }
}
