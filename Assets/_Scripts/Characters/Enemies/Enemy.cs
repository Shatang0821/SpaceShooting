using Assets.Scripts.Interface;
using EnemyManagment;
using Event;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Assets.Scripts.Characters.Enemies
{
    public class Enemy : Character, ITakenDamange
    {
        public GameObject EnemyPrefab { get; private set; }     // 敵オブジェクト

        private GameObject _enemyPrefab;
        public int ScorePoint { get; private set; }             // 倒す際にプレイヤーに与えるスコアポイント
        public int DeathEnergyBonus { get; private set; }       // 倒す際にプレイヤーに与えるエネルギーボーナス
        public float MoveSpeed { get; private set; }            //移動速度
        public Vector3 Padding { get; private set; }            // 敵サイズ
        public float MoveRotationAngele { get; private set; }   //回転角度
        public IEnemyBehavior Behavior { get; private set; }     //動きパターン

        public bool IsDied = false;
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
            this._enemyPrefab = enemyAircraft.EnemyPrefab;

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

        #region SET VALUE

        /// <summary>
        /// 位置設定
        /// </summary>
        /// <param name="pos"></param>
        public void SetPos(Vector3 pos)
        {
            Behavior.Initialize(EnemyPrefab);
            this.EnemyPrefab.transform.position = pos;
        }

        /// <summary>
        /// 行動設定
        /// </summary>
        /// <param name="behavior"></param>
        public void SetBehavior(IEnemyBehavior behavior)
        {

            this.Behavior = behavior;
        }

        /// <summary>
        /// クラスとゲームオブジェクトのアクティブ
        /// </summary>
        /// <param name="isActive"></param>
        public void SetActive(bool isActive)
        {
            this.EnemyPrefab = PoolManager.Release(_enemyPrefab);
            this.EnemyPrefab.SetActive(isActive);
            this.IsActive = isActive;
        }
        #endregion



        /// <summary>
        /// ダメージ処理
        /// </summary>
        /// <param name="damage"></param>
        public void TakenDamage(float damage)
        {
            this.Health -= damage;

            if (this.Health <= 0)
            {
                this.Health = 0;
                Die();
            }
        }

        /// <summary>
        /// 死亡処理
        /// </summary>
        public override void Die()
        {
            base.Die();
            IsDied = true;
            //Debug.Log("Die");
            EnemyPrefab.SetActive(false);
            // スコアマネージャーにスコアポイントを追加する
            ScoreManager.Instance.AddScore(ScorePoint);
            // プレイヤーエネルギーにエネルギーボーナスを追加する
            PlayerEnergy.Instance.Obtain(DeathEnergyBonus);
            
            EventCenter.TriggerEvent(EventKeyManager.UpdateRemoveList, this.EnemyPrefab);
            //ResetValues();

        }

        /// <summary>
        /// Enemyの数値をリセットするメソッド
        /// </summary>
        public void ResetValues()
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

            this.IsDied = false;
        }

        
    }
}
