using Assets.Scripts.Interface;
using System.Collections;
using UnityEngine;

namespace EnemyManagment
{
    public class EnemyBehavior : IEnemyBehavior
    {
        private Vector3 targetPosition;
        private GameObject _enemyPrefabs;
        private GameObject _projectile;
        private Transform _muzzleTransform;

        public void Move()
        {
            //if(Vector3.Distance(_enemyPrefabs.transform.position, targetPosition) >= _enemy.MoveSpeed * Time.fixedDeltaTime)
            //{
            //    // ターゲット位置に向かって移動
            //    _enemyPrefabs.transform.position = Vector3.MoveTowards(_enemyPrefabs.transform.position, targetPosition, _enemy.MoveSpeed * Time.fixedDeltaTime);

            //    // 敵が移動しているときに x 軸を回転させる
            //    //ターゲットのy座標と自身のy座標が異なると
            //    //Vector3.right=>x軸を中心に回転する　　　　　　　　　　　　　　　　　　　　　  normalized.yの結果によって高さ差が正であれば上を向き、高さ差が負であれば下を向くようになります
            //    _enemyPrefabs.transform.rotation = Quaternion.AngleAxis((targetPosition - _enemyPrefabs.transform.position).normalized.y * _enemy.MoveRotationAngele, Vector3.right);
            //}
            //else
            //{
            //    // 新しいターゲット位置を設定
            //    targetPosition = Viewport.Instance.RandomRightHalfPosition(_enemy.Padding);
            //}
        }

        public IEnumerator Attack()
        {
            //while (_enemyPrefabs.activeSelf)
            //{
            //    // ランダムな待機時間を設定
            //    yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));

            //    // ゲームがゲームオーバー状態であれば、コルーチンを終了 つまり、攻撃しないようにする
            //    if (GameManager.GameState == GameState.GameOver) yield break;

            //    // projectiles 配列内の各弾のプールからのリリース
            //    // 弾をプールからリリースし、muzzle の位置に配置
            //    PoolManager.Release(_projectile, _muzzleTransform.position);

            //    // ランダムな弾発射音を再生
            //    //AudioManager.Instance.PlayRandomSFX(projectileLaunchSFX);
            //}
            yield return null;
        }

    }
}
