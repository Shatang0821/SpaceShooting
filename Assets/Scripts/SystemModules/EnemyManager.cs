using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField] bool spawnEnemy = true;

    [SerializeField] GameObject[] enemyPrefabs;     //Enemy Prefabs

    //敵の生成間隔
    [SerializeField] float timeBetweenSpawns = 1f;  //Enemy Spawn interval

    //wave始まる前の準備時間
    [SerializeField] float timeBetweenWaves = 1f;

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
        waitUntilNoEnemy = new WaitUntil(() => enemyList.Count == 0);
    }

    //bool NoEnemy() => enemyList.Count == 0;

    IEnumerator Start()
    {
        //enemyがなくなるまで待機して、そして生成させる。初期listは敵入っていないから直接コルーチンが始まる
        while(spawnEnemy)
        {
            yield return waitUntilNoEnemy;
            yield return waitTimeBetweenWaves;
            yield return StartCoroutine(nameof(RandomlySpawnCoroutine));
        }
    }

    IEnumerator RandomlySpawnCoroutine()
    {
        //wave 3ごとに敵の数を増やしていく
        enemyAmount = Mathf.Clamp(enemyAmount, minEnemyAmout + waveNumber / 3, maxEnemyAmout);

        for (int i = 0; i < enemyAmount; i++)
        {
            //add in list
            enemyList.Add(PoolManager.Release(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]));

            yield return waitTimeBetweenSpawns;
        }

        waveNumber++;
    }

    //public void RemoveFromList(GameObject enemy)
    //{
    //    enemyList.Remove(enemy);
    //}

    public void RemoveFromList(GameObject enemy) => enemyList.Remove(enemy);
}
