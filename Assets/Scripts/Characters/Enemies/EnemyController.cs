using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("---- MOVE ----")]
    [SerializeField] float paddingX;

    [SerializeField] float paddingY;

    //�ړ����x
    [SerializeField] float moveSpeed = 2f;

    //��]�p�x
    [SerializeField] float moveRotationAngele = 25f;

    [Header("---- FIRE ----")]
    //�e�̃I�u�W�F�N�g
    [SerializeField] GameObject[] projectiles;

    //�e���ʒu
    [SerializeField] Transform muzzle;

    //�ŏ��U���Ԋu�E�ő�U���Ԋu
    [SerializeField] float minFireInterval;
    [SerializeField] float maxFireInterval;
    //pool�ɓ���邽�߃A�N�e�B�u��ԂɂȂ�����J�n
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
        //�G�̈ʒu�������_���ɃZ�b�g
        transform.position = Viewport.Instance.RandomEnemySpawnPosition(paddingX, paddingY);

        //�ړ�������߂�
        Vector3 targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);

        while(gameObject.activeSelf)
        {
            //if has not arrived targetPostion
            if(Vector3.Distance(transform.position,targetPosition) > Mathf.Epsilon)//Mathf.Epsilon�@0�Ɍ���Ȃ��߂Â�float�^���ł�
            {
                //keep moving to targetPosition
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                
                //�G���ړ����Ă���Ƃ���x��]������
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
