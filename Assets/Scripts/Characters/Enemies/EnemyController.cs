using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("---- MOVE ----")]
    float paddingX;

    float paddingY;

    //�ړ����x
    [SerializeField] float moveSpeed = 2f;

    //��]�p�x
    [SerializeField] float moveRotationAngele = 25f;

    [Header("---- FIRE ----")]
    //�e�̃I�u�W�F�N�g
    [SerializeField] GameObject[] projectiles;
    [SerializeField] AudioData[] projectileLaunchSFX;

    //�e���ʒu
    [SerializeField] Transform muzzle;

    //�ŏ��U���Ԋu�E�ő�U���Ԋu
    [SerializeField] float minFireInterval;
    [SerializeField] float maxFireInterval;

    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    private void Awake()
    {
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x / 2f;
        paddingY = size.y / 2f;
    }
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
            if(Vector3.Distance(transform.position,targetPosition) >= moveSpeed * Time.fixedDeltaTime)//Mathf.Epsilon�@0�Ɍ���Ȃ��߂Â�float�^���ł�
            {
                //keep moving to targetPosition
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
                
                //�G���ړ����Ă���Ƃ���x��]������
                transform.rotation = Quaternion.AngleAxis((targetPosition - transform.position).normalized.y * moveRotationAngele, Vector3.right);
            }
            else
            {
                //set a new targetPosition
                targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);
            }


            yield return waitForFixedUpdate;
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

            AudioManager.Instance.PlayRandomSFX(projectileLaunchSFX);
        }
    }
}
