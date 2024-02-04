using Assets._Scripts.Pool_System;
using Assets.Scripts.Characters.Enemies;
using Assets.Scripts.Interface;
using System.Collections.Generic;
using UnityEngine;


namespace EnemyManagment
{
    public class EnemyFactory
    {
        private const int SIZE = 50;                    //Enemyインスタンスサイズ

        private EnemyPool _enemyPool;                   //エネミーインスタンスプールを作成

        private Dictionary<AircraftType, EnemyAircraft> _enemyAircraftDictionary;
        public EnemyFactory()
        {
            _enemyAircraftDictionary = new Dictionary<AircraftType, EnemyAircraft>();
            _enemyPool = new EnemyPool(SIZE);

            foreach (var enemyData in DataManager.Instance.EnemyAircraftDatas)
            {
                var aircraft = new EnemyAircraft(enemyData);
                _enemyAircraftDictionary.Add(enemyData.Type, aircraft);

            }
        }



        /// <summary>
        /// 指定タイプのデータを返す
        /// </summary>
        /// <param name="aircraftType">機体タイプ</param>
        /// <returns></returns>
        public Enemy GetEnemy(AircraftType aircraftType)
        {
            var enemy = _enemyPool.AvaliableEnemy();
            enemy.Initialize(_enemyAircraftDictionary[aircraftType]);
            var behaviour = new EnemyBehavior(_enemyAircraftDictionary[aircraftType]);
            enemy.SetBehavior(behaviour);
            return enemy;
        }
    }
}

