using UnityEngine;

namespace EnemyManagment
{
    public class EnemyAircraft
    {
        public GameObject[] ProjectilePrefabs;      //  敵弾
        public GameObject EnemyPrefab;              //  敵プレハブ
        public float MaxHealth;                     //  最大HP
        public bool ShowOnHeadHealthBar;            //  HPBarの表示
        public int ScorePoint;                      //  倒す際にプレイヤーに与えるスコアポイント
        public int DeathEnergyBonus;                //  倒す際にプレイヤーに与えるエネルギーボーナス
        public float MoveSpeed;                     //  移動速度
        public Vector3 Padding;                     // 敵サイズ
        public float MoveRotationAngele;            //  回転角度

        public EnemyAircraft(EnemyAircraftData enemyAircraftData)
        {
            this.ProjectilePrefabs = enemyAircraftData.ProjectilePrefabs;
            this.EnemyPrefab = enemyAircraftData.EnemyPrefab;
            this.MaxHealth = enemyAircraftData.MaxHealth;
            this.ShowOnHeadHealthBar = enemyAircraftData.ShowOnHeadHealthBar;
            this.ScorePoint = enemyAircraftData.ScorePoint;
            this.DeathEnergyBonus = enemyAircraftData.DeathEnergyBonus;
            this.MoveSpeed = enemyAircraftData.MoveSpeed;

            this.Padding = enemyAircraftData.EnemyPrefab.transform.GetChild(0).GetComponent<Renderer>().bounds.size / 2f;// 敵のコライダーからサイズを計算
            this.MoveRotationAngele = enemyAircraftData.MoveRotationAngele;
        }
    }
}
