using UnityEngine;

namespace Assets.Scripts.Characters.Enemies
{
    public class Enemy : Character
    {
        public GameObject EnemyPrefab { get; private set; }    // 敵オブジェクト
        public int ScorePoint { get; private set; }             // 倒す際にプレイヤーに与えるスコアポイント
        public int DeathEnergyBonus { get; private set; }       // 倒す際にプレイヤーに与えるエネルギーボーナス
        public Vector3 Padding { get; private set; }            // 敵サイズ
        public float MoveSpeed { get; private set; }            //移動速度
        public float MoveRotationAngele {  get; private set; }  //回転角度

        public Enemy(GameObject enemy,float maxHealth, bool showOnHeadHealthBar,int scorePoint,int deathEnergyBonus,float moveSpeed) : base(maxHealth, showOnHeadHealthBar)
        {
            this.EnemyPrefab = enemy;
            this.ScorePoint = scorePoint;
            this.DeathEnergyBonus = deathEnergyBonus;
            this.Padding = enemy.transform.GetChild(0).GetComponent<Renderer>().bounds.size / 2f;    // 敵のコライダーからサイズを計算
            this.MoveSpeed = moveSpeed;
            this.MoveRotationAngele = 30f;

            Debug.Log(Padding);
        }

        public Enemy() { }
    }
}
