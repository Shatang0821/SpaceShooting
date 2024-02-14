using Assets.Scripts.Interface;
using System.Collections;
using UnityEngine;

namespace EnemyManagment
{
    public class EnemyBehavior : IEnemyBehavior
    {
        private Vector3 _targetPosition;
        private GameObject[] _projectiles;
        private GameObject _muzzle;

        private float _moveSpeed;
        private float _moveRotationAngle;
        private Vector3 _padding;

        private GameObject _enemy;
        public EnemyBehavior(EnemyAircraft enemyAircraft) 
        {
            this._projectiles = enemyAircraft.ProjectilePrefabs;
            this._moveSpeed = enemyAircraft.MoveSpeed;
            this._moveRotationAngle = enemyAircraft.MoveRotationAngele;
            this._padding = enemyAircraft.Padding;
        }

        public void Initialize(GameObject gameObject)
        {
            this._enemy = gameObject;
            _muzzle = gameObject.transform.Find("Muzzle").gameObject;
        }

        public void Move()
        {
            if (Vector3.Distance(_enemy.transform.position, _targetPosition) >= _moveSpeed * Time.fixedDeltaTime)
            {
                // ターゲット位置に向かって移動
                _enemy.transform.position = Vector3.MoveTowards(_enemy.transform.position, _targetPosition, _moveSpeed * Time.fixedDeltaTime);

                // 敵が移動しているときに x 軸を回転させる
                //ターゲットのy座標と自身のy座標が異なると
                //Vector3.right=>x軸を中心に回転する　　　　　　　　　　　　　　　　　　　　　  normalized.yの結果によって高さ差が正であれば上を向き、高さ差が負であれば下を向くようになります
                _enemy.transform.rotation = Quaternion.AngleAxis((_targetPosition - _enemy.transform.position).normalized.y * _moveRotationAngle, Vector3.right);
            }
            else
            {
                // 新しいターゲット位置を設定
                _targetPosition = Viewport.Instance.RandomRightHalfPosition(_padding);
            }
        }

        public IEnumerator Attack()
        {
            while (_enemy.activeSelf)
            {
                // ランダムな待機時間を設定
                yield return new WaitForSeconds(0.5f);

                // ゲームがゲームオーバー状態であれば、コルーチンを終了 つまり、攻撃しないようにする
                if (GameManager.GameState == GameState.GameOver) yield break;

                // projectiles 配列内の各弾のプールからのリリース
                // 弾をプールからリリースし、muzzle の位置に配置
                foreach(var p in _projectiles)
                {
                    PoolManager.Release(p, _muzzle.transform.position);
                }
                // ランダムな弾発射音を再生
                //AudioManager.Instance.PlayRandomSFX(projectileLaunchSFX);
            }
            yield return null;
        }

    }
}
