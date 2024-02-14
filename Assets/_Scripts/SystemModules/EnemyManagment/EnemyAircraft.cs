using UnityEngine;

namespace EnemyManagment
{
    /// <summary>
    /// EnemyDataをクラス化
    /// </summary>
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

        public EnemyAircraft(EnemyData enemyData)
        {
            this.ProjectilePrefabs = enemyData.ProjectilePrefabs;
            this.EnemyPrefab = enemyData.EnemyPrefab;
            this.MaxHealth = enemyData.MaxHealth;
            this.ShowOnHeadHealthBar = enemyData.ShowOnHeadHealthBar;
            this.ScorePoint = enemyData.ScorePoint;
            this.DeathEnergyBonus = enemyData.DeathEnergyBonus;
            this.MoveSpeed = enemyData.MoveSpeed;

            this.Padding = enemyData.EnemyPrefab.transform.GetChild(0).GetComponent<Renderer>().bounds.size / 2f;// 敵のコライダーからサイズを計算
            this.MoveRotationAngele = enemyData.MoveRotationAngele;
        }
    }
}
