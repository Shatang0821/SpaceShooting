﻿using Assets.Scripts.Characters.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.Pool_System
{
    public class EnemyPool
    {
        private Queue<Enemy> enemyQueue;

        public int Size {  get; private set; }

        public int RuntimeSize => enemyQueue.Count;

        public EnemyPool(int size) 
        {
            this.Size = size;
            Initialize();
        }
        /// <summary>
        /// Enemyキューの初期化
        /// </summary>
        private void Initialize()
        {
            enemyQueue = new Queue<Enemy>();

            for(var i = 0; i < Size; i++)
            { 
                enemyQueue.Enqueue(Copy());
            }
        }

        /// <summary>
        /// Enemyのインスタンス作成
        /// </summary>
        /// <returns></returns>
        private Enemy Copy()
        {
            var copy = new Enemy(false);

            return copy;
        }

        /// <summary>
        /// 使用可能なインスタンスを取得
        /// </summary>
        /// <returns></returns>
        private Enemy AvaliableEnemy()
        {
            Enemy avaliableEnemy = null;

            //キューの先頭から取り出す
            if(enemyQueue.Count > 0 && !enemyQueue.Peek().IsActive)
            {
                avaliableEnemy = enemyQueue.Dequeue();
            }
            else
            {
                avaliableEnemy = Copy();
            }

            //取り出したものを末尾に戻る
            enemyQueue.Enqueue (avaliableEnemy);

            return avaliableEnemy;
        }
    }
}
