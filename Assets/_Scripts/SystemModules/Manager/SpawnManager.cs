using Assets.Scripts.Characters.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{   
    public IEnumerator StartWave(Wave wave)
    {
        foreach(var spawn in wave.spawns)
        {
            yield return StartCoroutine(SpawnEnemy(spawn));
        }

    }

    private IEnumerator SpawnEnemy(SpawnData spawnData)
    {
        int number = 0;
        while(number < spawnData.SpawnNumber)
        {
            yield return new WaitForSeconds(spawnData.SpawnDelay);
            PoolManager.Release(spawnData.EnemyPrefab, spawnData.SpawnPoint);
            number++;
            yield return null;
        }
    }
}