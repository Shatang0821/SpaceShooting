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

        private Dictionary<EnemyData, EnemyAircraft> _enemyAircraftDictionary;
        public EnemyFactory()
        {
            Debug.Log("Create EnemyFactory Instance");
            _enemyAircraftDictionary = new Dictionary<EnemyData, EnemyAircraft>();
            _enemyPool = new EnemyPool(SIZE);

            foreach (var enemyData in DataManager.Instance.EnemyDatas)
            {
                var aircraft = new EnemyAircraft(enemyData);
                _enemyAircraftDictionary.Add(enemyData, aircraft);

            }
        }



        /// <summary>
        /// 指定タイプのデータを返す
        /// </summary>
        /// <param name="aircraftType">機体タイプ</param>
        /// <returns></returns>
        //public Enemy GetEnemy(EnemyType type)
        //{
        //    var enemy = _enemyPool.AvaliableEnemy();
        //    enemy.Initialize(_enemyAircraftDictionary[type]);
        //    var behaviour = new EnemyBehavior(_enemyAircraftDictionary[type]);
        //    enemy.SetBehavior(behaviour);
        //    return enemy;
        //}

        public Enemy GetEnemy(EnemyData data)
        {
            var enemy = _enemyPool.AvaliableEnemy();
            enemy.Initialize(_enemyAircraftDictionary[data]);
            var behaviour = new EnemyBehavior(_enemyAircraftDictionary[data]);
            enemy.SetBehavior(behaviour);
            if(enemy == null)
            {
                Debug.Log("Enemyインスタンス作成失敗しました");
            }
            return enemy;
        }
    }
}

