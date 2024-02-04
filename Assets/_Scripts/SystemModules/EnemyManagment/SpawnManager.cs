using UnityEngine;

namespace EnemyManagment
{
    public class SpawnManager
    {
        public SpawnManager() {
            Debug.Log("Create SpawnManager Instance");
        }

        public void Spawn(Wave wave)
        {
            var enemySpawnData =  wave.EnemySpawnDatas.Dequeue();
            for(int i = 0;i<enemySpawnData.EnemuNumber;i++)
            {
                GameObject.Instantiate(EnemyFactory.Instance.GetEnemyData(AircraftType.Aircraf01).EnemyPrefab,
                                        enemySpawnData.EnemySpawnPos, Quaternion.identity);
            }
            
        }
    }
}
