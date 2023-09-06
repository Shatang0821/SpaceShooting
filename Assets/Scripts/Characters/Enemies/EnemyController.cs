using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("---- MOVE ----")]
    [SerializeField] float paddingX;

    [SerializeField] float paddingY;

    //移動速度
    [SerializeField] float moveSpeed = 2f;

    //回転角度
    [SerializeField] float moveRotationAngele = 25f;

    [Header("---- FIRE ----")]
    //弾のオブジェクト
    [SerializeField] GameObject[] projectiles;

    //銃口位置
    [SerializeField] Transform muzzle;

    //最小攻撃間隔・最大攻撃間隔
    [SerializeField] float minFireInterval;
    [SerializeField] float maxFireInterval;
    //poolに入れるためアクティブ状態になったら開始
    void OnEnable()
    {
        StartCoroutine(nameof(RandomlyMovingCoroutine));
        StartCoroutine(nameof(RandomlyFireCoroutine));
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator RandomlyMovingCoroutine()
    {
        //敵の位置をランダムにセット
        transform.position = Viewport.Instance.RandomEnemySpawnPosition(paddingX, paddingY);

        //移動先を決める
        Vector3 targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);

        while(gameObject.activeSelf)
        {
            //if has not arrived targetPostion
            if(Vector3.Distance(transform.position,targetPosition) > Mathf.Epsilon)//Mathf.Epsilon　0に限りなく近づくfloat型数です
            {
                //keep moving to targetPosition
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                
                //敵が移動しているときのx回転させる
                transform.rotation = Quaternion.AngleAxis((targetPosition - transform.position).normalized.y * moveRotationAngele, Vector3.right);
            }
            else
            {
                //set a new targetPosition
                targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);
            }


            yield return null;
        }
    }

    IEnumerator RandomlyFireCoroutine()
    {
        while(gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));

            foreach(var projectile in projectiles)
            {
                PoolManager.Release(projectile, muzzle.position);
            }
        }
    }
}
