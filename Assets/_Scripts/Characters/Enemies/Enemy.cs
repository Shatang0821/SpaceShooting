using Assets.Scripts.Interface;
using EnemyManagment;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Assets.Scripts.Characters.Enemies
{
    public class Enemy : Character, ITakenDamange
    {
        public GameObject EnemyPrefab { get; private set; }     // 敵オブジェクト
        public int ScorePoint { get; private set; }             // 倒す際にプレイヤーに与えるスコアポイント
        public int DeathEnergyBonus { get; private set; }       // 倒す際にプレイヤーに与えるエネルギーボーナス
        public float MoveSpeed { get; private set; }            //移動速度
        public Vector3 Padding { get; private set; }            // 敵サイズ
        public float MoveRotationAngele { get; private set; }   //回転角度
        public IEnemyBehavior Behavior { get; private set; }     //動きパターン


        public Enemy(bool isActive) : base(isActive) {}
        /// <summary>
        /// Enemyのセット
        /// </summary>
        /// <param name="enemy">敵プレハブ</param>
        /// <param name="maxHealth">敵最大HP</param>
        /// <param name="showOnHeadHealthBar"></param>
        /// <param name="scorePoint"></param>
        /// <param name="deathEnergyBonus"></param>
        /// <param name="moveSpeed"></param>
        public void Initialize(EnemyAircraft enemyAircraft)
        {
            this.EnemyPrefab = PoolManager.Release(enemyAircraft.EnemyPrefab,Vector3.zero);
            this.EnemyPrefab.SetActive(true);

            this.MaxHealth = enemyAircraft.MaxHealth;
            this.Health = enemyAircraft.MaxHealth;
            this.ShowOnHeadHealthBar = enemyAircraft.ShowOnHeadHealthBar;
            this.ScorePoint = enemyAircraft.ScorePoint;
            this.DeathEnergyBonus = enemyAircraft.DeathEnergyBonus;
            this.MoveSpeed = enemyAircraft.MoveSpeed;
            this.Padding = enemyAircraft.Padding;
            this.MoveRotationAngele = enemyAircraft.MoveRotationAngele;

        }

        public void Update()
        {
            Behavior.Move();
        }

        public void SetEnemyPos(Vector3 pos)
        {
            this.EnemyPrefab.transform.position = pos;
        }

        public void SetBehavior(IEnemyBehavior behavior)
        {
            behavior.Initialize(EnemyPrefab);
            this.Behavior = behavior;
        }

        public void TakenDamage(float damage)
        {
            this.Health -= damage;

            if (this.Health <= 0)
            {
                this.Health = 0;
                Die();
            }
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
