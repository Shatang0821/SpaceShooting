using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    /// <summary>
    /// 敵をランダムに一つを取り出す
    /// </summary>
    public GameObject RandomEnemy => enemyList.Count == 0 ? null : enemyList[Random.Range(0, enemyList.Count)];

    public int WaveNumber => waveNumber;

    public float TimeBetweenWaves => timeBetweenWaves;

    [SerializeField] bool spawnEnemy = true;
    [SerializeField] GameObject waveUI;

    [SerializeField] GameObject[] enemyPrefabs;     //Enemy 敵の配列

    //敵の生成間隔
    [SerializeField] float timeBetweenSpawns = 1f;  //敵ごと生成間隔

    //wave始まる前の準備時間
    [SerializeField] float timeBetweenWaves = 1f;　//ウェーブ間隔

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
        //コルーチンをboolで終わらせる
        waitUntilNoEnemy = new WaitUntil(() => enemyList.Count == 0);
    }

    //bool NoEnemy() => enemyList.Count == 0;

    //ゲーム開始時自動的に実行するコルーチン
    IEnumerator Start()
    {
        //enemyがなくなるまで待機して、そして生成させる。初期listは敵入っていないから直接コルーチンが始まる
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
        //wave 3ごとに敵の数を増やしていく
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
