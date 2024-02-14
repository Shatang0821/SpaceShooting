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
        public SpawnManager() {
            _enemyFactory = new EnemyFactory();
            Debug.Log("Create SpawnManager Instance");
        }
        #region EnemyManager用

        //public List<Enemy> GetEnemy(EnemyType type,int count)
        //{
        //    var enemyList = new List<Enemy>();
        //    for (int i = 0; i < count; i++)
        //    {
        //        var enemy = _enemyFactory.GetEnemy(type);
        //        enemy.SetEnemyPos(new Vector3(10, i, 0));

        //        enemyList.Add(enemy);
        //    }
        //    return enemyList;
        //}
        #endregion
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
            //Debug.Log($"Enemy事前準備完了{prepareEnemies.Count}");
            return prepareEnemies;
        }
        
        public IEnumerator Spawn(List<Enemy> prepareEnemies)
        {
            int index = 0;
            while (_enemySpawnDatas.Count > 0)
            {
                var enemySpawnData = _enemySpawnDatas.Dequeue();
                for(int i = 0;i<enemySpawnData.EnemyNumber;i++)
                {
                    prepareEnemies[index].SetActive(true);
                    prepareEnemies[index].SetPos(enemySpawnData.EnemySpawnPos);
                    if (prepareEnemies[index].Behavior == null)
                    {
                        Debug.Log("Behaviorが設定していない");
                    }

                    yield return new WaitForSeconds(enemySpawnData.SpawnInterval);
                    index++;
                    Debug.Log(index);
                }

                yield return null;
            }

            Debug.Log("DONE");
        }
    }
}
