using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("---- MOVE ----")]
    float paddingX;

    float paddingY;

    //移動速度
    [SerializeField] float moveSpeed = 2f;

    /// <summary>
    /// 回転角度
    /// </summary>
    [SerializeField] float moveRotationAngele = 25f;

    [Header("---- FIRE ----")]
    //弾のオブジェクト
    [SerializeField] protected GameObject[] projectiles;
    [SerializeField] protected AudioData[] projectileLaunchSFX;

    //銃口位置
    [SerializeField] protected Transform muzzle;

    //最小攻撃間隔・最大攻撃間隔
    [SerializeField] protected float minFireInterval;
    [SerializeField] protected float maxFireInterval;

    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    protected virtual void Awake()
    {
        // 敵のコライダーからパディングを計算                     boundsからオブジェクトのサイズを取得する
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x / 2f;
        paddingY = size.y / 2f;
    }

    //poolに入れるためアクティブ状態になったら開始
    void OnEnable()
    {
        // ランダムな動きと攻撃のコルーチンを開始
        StartCoroutine(nameof(RandomlyMovingCoroutine));
        StartCoroutine(nameof(RandomlyFireCoroutine));
    }

    void OnDisable()
    {
        // すべてのコルーチンを停止
        StopAllCoroutines();
    }

    /// <summary>
    /// ランダムな動きをするコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator RandomlyMovingCoroutine()
    {
        //敵の位置をランダムにセット
        transform.position = Viewport.Instance.RandomEnemySpawnPosition(paddingX, paddingY);

        //移動先を決める
        Vector3 targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);

        //オブジェクトがアクティブな状態であるなら
        while(gameObject.activeSelf)
        {
            // ターゲット位置に到達していない場合
            // 距離が次のフレームでの移動距離よりも小さい場合、つまり、次のフレームでの移動がすでに目標位置に非常に近い場合
            //elseで新しい位置をセットする
            if (Vector3.Distance(transform.position,targetPosition) >= moveSpeed * Time.fixedDeltaTime)//Mathf.Epsilon　0に限りなく近づくfloat型数です
            {
                // ターゲット位置に向かって移動
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);

                // 敵が移動しているときに x 軸を回転させる
                //ターゲットのy座標と自身のy座標が異なると
                //Vector3.right=>x軸を中心に回転する　　　　　　　　　　　　　　　　　　　　　  normalized.yの結果によって高さ差が正であれば上を向き、高さ差が負であれば下を向くようになります
                transform.rotation = Quaternion.AngleAxis((targetPosition - transform.position).normalized.y * moveRotationAngele, Vector3.right);
            }
            else
            {
                // 新しいターゲット位置を設定
                targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);
            }

            //移動にかかわる演算だからFixedUpdateを使って精度を上げる
            yield return waitForFixedUpdate;
        }
    }

    /// <summary>
    /// 攻撃間隔設定
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator RandomlyFireCoroutine()
    {
        while(gameObject.activeSelf)
        {
            // ランダムな待機時間を設定
            yield return new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));

            // ゲームがゲームオーバー状態であれば、コルーチンを終了 つまり、攻撃しないようにする
            if (GameManager.GameState == GameState.GameOver) yield break;

            // projectiles 配列内の各弾のプールからのリリース
            foreach (var projectile in projectiles)
            {
                // 弾をプールからリリースし、muzzle の位置に配置
                PoolManager.Release(projectile, muzzle.position);
            }

            // ランダムな弾発射音を再生
            AudioManager.Instance.PlayRandomSFX(projectileLaunchSFX);
        }
    }
}
