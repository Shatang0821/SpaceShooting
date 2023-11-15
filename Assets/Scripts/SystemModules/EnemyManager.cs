using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    /// <summary>
    /// �G�������_���Ɉ�����o��
    /// </summary>
    public GameObject RandomEnemy => enemyList.Count == 0 ? null : enemyList[Random.Range(0, enemyList.Count)];

    public int WaveNumber => waveNumber;

    public float TimeBetweenWaves => timeBetweenWaves;

    [SerializeField] bool spawnEnemy = true;
    [SerializeField] GameObject waveUI;

    [SerializeField] GameObject[] enemyPrefabs;     //Enemy �G�̔z��

    //�G�̐����Ԋu
    [SerializeField] float timeBetweenSpawns = 1f;  //�G���Ɛ����Ԋu

    //wave�n�܂�O�̏�������
    [SerializeField] float timeBetweenWaves = 1f;�@//�E�F�[�u�Ԋu

    [SerializeField] int minEnemyAmout = 4;

    [SerializeField] int maxEnemyAmout = 10;

    int waveNumber = 1;

    int enemyAmount;

    List<GameObject> enemyList;

    WaitForSeconds waitTimeBetweenSpawns;

    WaitForSeconds waitTimeBetweenWaves;

    WaitUntil waitUntilNoEnemy;

    protected override void Awake()
    {
        base.Awake();
        enemyList = new List<GameObject>();
        waitTimeBetweenSpawns = new WaitForSeconds(timeBetweenSpawns);
        waitTimeBetweenWaves = new WaitForSeconds(timeBetweenWaves);
        //waitUntilNoEnemy = new WaitUntil(NoEnemy);
        //�R���[�`����bool�ŏI��点��
        waitUntilNoEnemy = new WaitUntil(() => enemyList.Count == 0);
    }

    //bool NoEnemy() => enemyList.Count == 0;

    //�Q�[���J�n�������I�Ɏ��s����R���[�`��
    IEnumerator Start()
    {
        //enemy���Ȃ��Ȃ�܂őҋ@���āA�����Đ���������B����list�͓G�����Ă��Ȃ����璼�ڃR���[�`�����n�܂�
        while(spawnEnemy)
        {
            waveUI.SetActive(true);

            yield return waitTimeBetweenWaves;

            waveUI.SetActive(false);
            yield return StartCoroutine(nameof(RandomlySpawnCoroutine));
        }
    }

    IEnumerator RandomlySpawnCoroutine()
    {
        //wave 3���ƂɓG�̐��𑝂₵�Ă���
        enemyAmount = Mathf.Clamp(enemyAmount, minEnemyAmout + waveNumber / 3, maxEnemyAmout);

        for (int i = 0; i < enemyAmount; i++)
        {
            //add in list
            enemyList.Add(PoolManager.Release(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]));

            yield return waitTimeBetweenSpawns;
        }

        yield return waitUntilNoEnemy;

        waveNumber++;
    }

    //public void RemoveFromList(GameObject enemy)
    //{
    //    enemyList.Remove(enemy);
    //}

    public void RemoveFromList(GameObject enemy) => enemyList.Remove(enemy);
}
