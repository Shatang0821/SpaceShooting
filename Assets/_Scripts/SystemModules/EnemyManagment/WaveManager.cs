using Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EnemyManagment
{
    /*
        敵のWave管理するクラス
        それぞれのWaveで、
        どの敵がどの位置から
        どれぐらいの間隔で
        どれほどの数で出現するかを制御する
    */

    public enum WaveState
    {
        Waiting,
        InProgress,
        Completed
    }
    public class WaveManager
    {
        public WaveState CurrentState { get; private set; } //現在状態

        //ステージのWave情報
        private Queue<Wave> _waves;                         //

        //現在のWave(複数Waveがある前提)
        private Wave currentWave;

        public WaveManager()
        {
            Debug.Log("Create WaveManager Instance");
            _waves = new Queue<Wave> { };
            CurrentState = WaveState.Waiting;
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize()
        {
            Debug.Log("WaveManager Initialize");
            //ステージのWaveデータ
            foreach (var waveData in DataManager.Instance.EnemyWaveDatas)
            {
                var wave = new Wave(waveData.EnemySpawnDatas);
                _waves.Enqueue(wave);
            }
        }

        public EnemySpawnData GetSpawnData()
        {
            if(!currentWave.IsCompleted())
            {
                return currentWave.GetSpawnData();
            }
            else
            {
                Debug.LogWarning("EnemySpawnData IS None");
                return null;
            }
        }
    }

    public class Wave
    {
        private Queue<EnemySpawnData> EnemySpawnDatas;

        public Wave(EnemySpawnData[] enemySpawnDatas)
        {
            EnemySpawnDatas = new Queue<EnemySpawnData>();
            foreach (var data in enemySpawnDatas)
            {
                EnemySpawnDatas.Enqueue(data);
            }
        }
        /// <summary>
        /// 敵キューが空になったかどうか
        /// </summary>
        /// <returns></returns>
        public bool IsCompleted()
        {
            return EnemySpawnDatas.Count == 0;
        }

        public EnemySpawnData GetSpawnData()
        {
            if(EnemySpawnDatas.Count == 0)
            {
                Debug.LogWarning("空チェックが正常に動作していない");
                return null;
            }
            else
            {
                return EnemySpawnDatas.Dequeue();
            }
        }
    }
}
