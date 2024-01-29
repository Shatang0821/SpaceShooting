using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Characters.Enemies
{
    public class SpawnData
    {
        public GameObject EnemyPrefab { get; private set; } // 敵のプレハブ
        public Vector2 SpawnPoint { get; private set; }      // 出現位置
        public float SpawnDelay { get; private set; }       // 出現までの遅延時間（秒）
        public int SpawnNumber { get; private set; }        //敵数

        /// <summary>
        /// 生成データコンストラクター
        /// </summary>
        /// <param name="enemyPrefab">生成する敵</param>
        /// <param name="spawnPoint">生成する位置</param>
        /// <param name="spawnDelay">生成する間隔</param>
        /// <param name="spawnNumber">生成する数</param>
        public SpawnData(GameObject enemyPrefab, Vector2 spawnPoint, float spawnDelay, int spawnNumber)
        {
            this.EnemyPrefab = enemyPrefab;
            this.SpawnPoint = spawnPoint;
            this.SpawnDelay = spawnDelay;
            this.SpawnNumber = spawnNumber;
        }
    }
}
