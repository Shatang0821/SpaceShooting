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

    [SerializeField] float timeBetweenSpawns = 3f;  //Wave���Ɛ����Ԋu


    private List<Enemy> _enemyList = new List<Enemy>();

    private List<Enemy> _activeEnemyList = new List<Enemy>();

    private List<Enemy> _enemiesToRemove = new List<Enemy>();

    private Dictionary<GameObject, Enemy> _enemyDictionary = new Dictionary<GameObject, Enemy>();

    private WaitForSeconds _waitTimeBetweenSpawns;
    protected override void Awake()
    {
        base.Awake();

        Initialize();
    }

    /// <summary>
    /// �C���X�^���X�쐬�Ə�����
    /// </summary>
    private void Initialize()
    {
        _spawnManager = new SpawnManager();
        _waveManager = new WaveManager();

        _waitTimeBetweenSpawns = new WaitForSeconds(timeBetweenSpawns);

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
        EventCenter.Subscribe(EventKeyManager.UpdateRemoveList, UpdateRemoveList);
    }

    private void OnDisable()
    {
        EventCenter.Unsubscribe(EventKeyManager.UpdateRemoveList, UpdateRemoveList);
    }

    /// <summary>
    /// Wave�̐ݒ�
    /// </summary>
    /// <returns></returns>
    private IEnumerator InWaiting()
    {
        StartWave();

        yield return null;
    }

    /// <summary>
    /// wave�̏���
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

        Debug.Log("EnemyList�̐��� : " + _enemyList.Count);
        Debug.Log("EnemiesToRemove�̐��� : " + _enemiesToRemove.Count);
        Debug.Log("EnemyDictionaryt�̐��� : " + _enemyDictionary.Count);
    }

    /// <summary>
    /// �G���s�X�V
    /// </summary>
    /// <returns></returns>
    private IEnumerator InProgressUpdate()
    {
        EventCenter.TriggerEvent(UIEventKey.ShowWave, _waveManager.WaveCount);
        //UI��҂�
        yield return _waitTimeBetweenSpawns;
        EventCenter.TriggerEvent(UIEventKey.HideWave);

        StartCoroutine(_spawnManager.Spawn(_enemyList, _enemyDictionary));
        while (IsProgressUpdate)
        {
            foreach (var enemy in _enemyList)
            {
                if (enemy.EnemyPrefab != null)
                {
                    enemy.Update();
                }
            }
            if(!_spawnManager.IsSpawning)
            {
                Remove();
            }
            yield return null;
        }
        if(_enemiesToRemove.Count > 0 && _enemyDictionary.Count > 0) 
        {
            Debug.Log("ERROR");
        }
        _waveManager.UpdateIndex();
        _currentState = EnemyManagerState.Waiting;
    }

    /// <summary>
    /// �Q�[���I��
    /// </summary>
    /// <returns></returns>
    private IEnumerator InCompleted()
    {
        Reset();
        //Debug.Log("�Q�[���N���A");
        yield return null;
    }

    private bool IsProgressUpdate => _enemyList.Count > 0 && GameManager.GameState != GameState.GameOver;

    /// <summary>
    /// ���X�g����s�v�ȃI�u�W�F�N�g���폜
    /// </summary>
    void UpdateRemoveList(object obj)
    {
        if(obj is GameObject)
        {
            var enemy = _enemyDictionary[(GameObject)obj];
            _enemiesToRemove.Add(enemy);
        }
    }

    void Remove()
    {
        if(_enemiesToRemove.Count > 0)
        {
            foreach(var enemy in _enemiesToRemove) 
            {
                _enemyList.Remove(enemy);
                _enemyDictionary.Remove(enemy.EnemyPrefab);
            }
        }
        _enemiesToRemove.Clear();
    }
    public void Damage(GameObject gameObject, float damage)
    {
        // �L�[�������ɑ��݂��邩���m�F
        if (_enemyDictionary.ContainsKey(gameObject))
        {
            var enemy = _enemyDictionary[gameObject];
            // IsActive�̃`�F�b�N�����ŏ\��
            if (enemy.IsActive)
            {
                enemy.TakenDamage(damage);
            }
        }
        else
        {
            // gameObject��_enemyDictionary�ɑ��݂��Ȃ��ꍇ�̏����i���O�L�^�Ȃǁj
            Debug.LogWarning($"Enemy not found for gameObject {gameObject.name}");
        }

    }
    /// <summary>
    /// �U���J�n�Ǝ����o�^
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
    /// �G�������_���Ɉ�����o��
    /// </summary>
    public GameObject RandomEnemy => _activeEnemyList.Count == 0 ? null : _activeEnemyList[Random.Range(0, _activeEnemyList.Count)].EnemyPrefab;
}
