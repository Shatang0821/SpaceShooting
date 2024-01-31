using Event;
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
    public class WaveManager
    {
        private static WaveManager _instance;
        public static WaveManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    Debug.Log("Create WaveManager Instance");
                    _instance = new WaveManager();
                }
                return _instance;
            }
        }

        //ステージのWave情報
        private Queue<Wave> _waves;

        public WaveManager()
        {
        }


        public void Initialize()
        {
            Debug.Log("WaveManager Initialize");
            _waves = new Queue<Wave> { };
            foreach (var waveData in DataManager.Instance.EnemyWaveDatas)
            {
                var wave = new Wave(waveData.EnemySpawnDatas);
                _waves.Enqueue(wave);
                
            }
        }

        public void StartWave()
        {
            Debug.Log("Start First Wave");
        }

        public void UpdateWave()
        {

        }

    }

    public class Wave
    {
        public EnemySpawnData[] EnemySpawnDatas;

        public Wave(EnemySpawnData[] enemySpawnDatas) 
        {
            EnemySpawnDatas = enemySpawnDatas;
        }
    }
}
