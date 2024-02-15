using Assets.Scripts.Characters.Enemies;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace EnemyManagment
{
    public class SpawnManager
    {
        private EnemyFactory _enemyFactory;

        private Queue<EnemySpawnData> _enemySpawnDatas = new Queue<EnemySpawnData>();

        public bool IsSpawning {  get; private set; }
        public SpawnManager() 
        {
            IsSpawning = false;
            _enemyFactory = new EnemyFactory();
            Debug.Log("Create SpawnManager Instance");

        }
        public List<Enemy> PrepareEnemies(Wave wave)
        {
            var prepareEnemies = new List<Enemy>();
            while(!wave.IsCompleted())
            {
               var spawnData = wave.GetSpawnData();
                _enemySpawnDatas.Enqueue(spawnData);
               for(int i = 0;i < spawnData.EnemyNumber; i++) 
                {
                    prepareEnemies.Add(_enemyFactory.GetEnemy(spawnData.EnemyData));
                }
            }
            //Debug.Log($"EnemyŽ–‘O€”õŠ®—¹{prepareEnemies.Count}");
            return prepareEnemies;
        }
        
        public IEnumerator Spawn(List<Enemy> prepareEnemies, Dictionary<GameObject,Enemy> enemyDictionary)
        {
            IsSpawning = true;
            int index = 0;
            while (_enemySpawnDatas.Count > 0)
            {
                var enemySpawnData = _enemySpawnDatas.Dequeue();
                for(int i = 0;i<enemySpawnData.EnemyNumber;i++)
                {
                    prepareEnemies[index].SetActive(true);
                    prepareEnemies[index].SetPos(enemySpawnData.EnemySpawnPos);
                    enemyDictionary.Add(prepareEnemies[index].EnemyPrefab, prepareEnemies[index]);

                    if (prepareEnemies[index].Behavior == null)
                    {
                        Debug.Log("Behavior‚ªÝ’è‚µ‚Ä‚¢‚È‚¢");
                    }

                    
                    yield return new WaitForSeconds(enemySpawnData.SpawnInterval);
                    index++;
                    //Debug.Log(index);
                }

                yield return null;
            }

            IsSpawning = false;
            //Debug.Log("DONE");
        }
    }
}
