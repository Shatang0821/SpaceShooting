using Assets._Scripts.Pool_System;
using Assets.Scripts.Characters.Enemies;
using Assets.Scripts.Interface;
using System.Collections.Generic;
using UnityEngine;


namespace EnemyManagment
{
    public class EnemyFactory
    {
        private const int SIZE = 50;                    //Enemy�C���X�^���X�T�C�Y

        private EnemyPool _enemyPool;                   //�G�l�~�[�C���X�^���X�v�[�����쐬

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
        /// �w��^�C�v�̃f�[�^��Ԃ�
        /// </summary>
        /// <param name="aircraftType">�@�̃^�C�v</param>
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

