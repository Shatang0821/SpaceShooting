using Assets.Scripts.Characters.Enemies;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyManagment
{
    public class SpawnManager
    {
        private EnemyFactory _enemyFactory;
        public SpawnManager() {
            _enemyFactory = new EnemyFactory();
            Debug.Log("Create SpawnManager Instance");
        }
        #region EnemyManager—p
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Enemy> GetEnemy(AircraftType type,int count)
        {
            var enemyList = new List<Enemy>();
            for (int i = 0; i < count; i++)
            {
                var enemy = _enemyFactory.GetEnemy(type);
                enemy.SetEnemyPos(new Vector3(10, i, 0));

                enemyList.Add(enemy);
            }
            return enemyList;
        }
        #endregion
        public void Spawn(Wave wave)
        {
           
            
        }
    }
}
