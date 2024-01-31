using Assets._Scripts.Pool_System;
using System.Collections.Generic;
using UnityEngine;


namespace EnemyManagment
{
    public class EnemyFactory
    {
        private static EnemyFactory _instance;


        private const int SIZE = 50;                    //Enemyインスタンスサイズ

        private EnemyPool _enemyPool;                   //エネミーインスタンスプールを作成

        private Dictionary<AircraftType, EnemyAircraft> _enemyAircraftDictionary;

        public EnemyFactory()
        {
            _enemyAircraftDictionary = new Dictionary<AircraftType, EnemyAircraft>();
            _enemyPool = new EnemyPool(SIZE);

            foreach (var enemyData in DataManager.Instance.EnemyAircraftDatas)
            {
                var aircraft = new EnemyAircraft(enemyData.EnemyPrefab, enemyData.ProjectilePrefabs);
                _enemyAircraftDictionary.Add(enemyData.Type, aircraft);
            }
        }

        public static EnemyFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EnemyFactory();
                }
                return _instance;
            }
        }

        /// <summary>
        /// 指定タイプのデータを返す
        /// </summary>
        /// <param name="aircraftType">機体タイプ</param>
        /// <returns></returns>
        public EnemyAircraft GetEnemyData(AircraftType aircraftType)
        {
            return _enemyAircraftDictionary[aircraftType];
        }
    }
}

