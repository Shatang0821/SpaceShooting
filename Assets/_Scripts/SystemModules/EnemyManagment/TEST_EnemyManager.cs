using Event;
using EnemyManagment;
using UnityEngine;
using Assets.Scripts.Interface;
using Assets.Scripts.Characters.Enemies;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine.Rendering;

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

    private List<Enemy> _activeEnemyList = new List<Enemy>();
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

    private void OnEnable()
    {
        EventCenter.Subscribe(EventKeyManager.RemoveEnemy, Remove);
    }

    private void OnDisable()
    {
        EventCenter.Unsubscribe(EventKeyManager.RemoveEnemy, Remove);
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
            //SetDictionary(_enemyList);

            _currentState = EnemyManagerState.InProgress;
        }
        else
        {
            _currentState = EnemyManagerState.Completed;
        }

        Debug.Log("EnemyListの数は : " + _enemyList.Count);
    }

    /// <summary>
    /// 敵実行更新
    /// </summary>
    /// <returns></returns>
    private IEnumerator InProgressUpdate()
    {
        StartCoroutine(_spawnManager.Spawn(_enemyList, _enemyDictionary));
        while (IsProgressUpdate)
        {
            _enemiesToRemove.Clear();

            foreach (var enemy in _enemyList)
            {
                if (enemy.EnemyPrefab != null)
                {
                    enemy.Update();
                }
            }
                         
            yield return new WaitForFixedUpdate();
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
        Reset();
        Debug.Log("ゲームクリア");
        yield return null;
    }

    private bool IsProgressUpdate => _enemyList.Count > 0 && GameManager.GameState != GameState.GameOver;

    /// <summary>
    /// リストから不要なオブジェクトを削除
    /// </summary>
    void Remove(object obj)
    {
        if(obj is GameObject)
        {
            var enemy = _enemyDictionary[(GameObject)obj];
            enemy.ResetValues();
            _enemyList.Remove(enemy);
            _enemyDictionary.Remove((GameObject)obj);
            
        }
    }

    public void Damage(GameObject gameObject, float damage)
    {
        // キーが辞書に存在するかを確認
        if (_enemyDictionary.ContainsKey(gameObject))
        {
            var enemy = _enemyDictionary[gameObject];
            // IsActiveのチェックだけで十分
            if (enemy.IsActive)
            {
                enemy.TakenDamage(damage);
            }
        }
        else
        {
            // gameObjectが_enemyDictionaryに存在しない場合の処理（ログ記録など）
            Debug.LogWarning($"Enemy not found for gameObject {gameObject.name}");
        }

    }
    /// <summary>
    /// 攻撃開始と辞書登録
    /// </summary>
    /// <param name="enemies"></param>
    //public void SetDictionary(List<Enemy> enemies)
    //{
    //    foreach (var enemy in enemies)
    //    {
    //        if (!_enemyDictionary.ContainsKey(enemy.EnemyPrefab))
    //        {
    //            _enemyDictionary.Add(enemy.EnemyPrefab, enemy);
    //            StartCoroutine(enemy.Behavior.Attack());
    //        }
            
    //    }
    //}

    private void Reset()
    {
        _enemyList.Clear();
        _activeEnemyList.Clear();
        _enemiesToRemove.Clear();
        _enemyDictionary.Clear();

    }
    /// <summary>
    /// 敵をランダムに一つを取り出す
    /// </summary>
    public GameObject RandomEnemy => _activeEnemyList.Count == 0 ? null : _activeEnemyList[Random.Range(0, _activeEnemyList.Count)].EnemyPrefab;
}
