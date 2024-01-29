using UnityEngine;

namespace Assets.Scripts.Characters.Enemies
{
    public class Enemy : Character
    {
        public GameObject EnemyPrefab { get; private set; }    // 敵オブジェクト
        public int ScorePoint { get; private set; }             // 倒す際にプレイヤーに与えるスコアポイント
        public int DeathEnergyBonus { get; private set; }       // 倒す際にプレイヤーに与えるエネルギーボーナス
        public float MoveSpeed { get; private set; }            //移動速度
        public Vector3 Padding { get; private set; }            // 敵サイズ
        public float MoveRotationAngele { get; private set; }  //回転角度

        public Enemy(bool isActive) : base(isActive) { }

        /// <summary>
        /// Enemyのセット
        /// </summary>
        /// <param name="enemy">敵プレハブ</param>
        /// <param name="maxHealth">敵最大HP</param>
        /// <param name="showOnHeadHealthBar"></param>
        /// <param name="scorePoint"></param>
        /// <param name="deathEnergyBonus"></param>
        /// <param name="moveSpeed"></param>
        public void Initialize(EnemyAircraftData enemyAircraftData)
        {
            this.EnemyPrefab = enemyAircraftData.EnemyPrefab;
            this.MaxHealth = enemyAircraftData.MaxHealth;
            this.Health = enemyAircraftData.MaxHealth;
            this.ShowOnHeadHealthBar = enemyAircraftData.ShowOnHeadHealthBar;
            this.ScorePoint = enemyAircraftData.ScorePoint;
            this.DeathEnergyBonus = enemyAircraftData.DeathEnergyBonus;
            this.MoveSpeed = enemyAircraftData.MoveSpeed;
            this.Padding = enemyAircraftData.EnemyPrefab.transform.GetChild(0).GetComponent<Renderer>().bounds.size / 2f;// 敵のコライダーからサイズを計算
            this.MoveRotationAngele = enemyAircraftData.MoveRotationAngele;

        }

        public override void Die()
        {
            base.Die();
            // スコアマネージャーにスコアポイントを追加する
            ScoreManager.Instance.AddScore(ScorePoint);
            // プレイヤーエネルギーにエネルギーボーナスを追加する
            PlayerEnergy.Instance.Obtain(DeathEnergyBonus);

            EnemyPrefab.SetActive(false);

            ResetValues();
        }

        /// <summary>
        /// Enemyの数値をリセットするメソッド
        /// </summary>
        private void ResetValues()
        {
            this.EnemyPrefab = null;
            this.MaxHealth = 0;
            this.Health = 0;
            this.ShowOnHeadHealthBar = false;
            this.ScorePoint = 0;
            this.DeathEnergyBonus = 0;
            this.MoveSpeed = 0;
            this.Padding = Vector3.zero;
            this.MoveRotationAngele = 0;
        }
    }
}
