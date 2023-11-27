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

    /// <summary>
    /// ��]�p�x
    /// </summary>
    [SerializeField] float moveRotationAngele = 25f;

    [Header("---- FIRE ----")]
    //�e�̃I�u�W�F�N�g
    [SerializeField] protected GameObject[] projectiles;
    [SerializeField] protected AudioData[] projectileLaunchSFX;

    //�e���ʒu
    [SerializeField] protected Transform muzzle;

    //�ŏ��U���Ԋu�E�ő�U���Ԋu
    [SerializeField] protected float minFireInterval;
    [SerializeField] protected float maxFireInterval;

    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    protected virtual void Awake()
    {
        // �G�̃R���C�_�[����p�f�B���O���v�Z                     bounds����I�u�W�F�N�g�̃T�C�Y���擾����
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x / 2f;
        paddingY = size.y / 2f;
    }

    //pool�ɓ���邽�߃A�N�e�B�u��ԂɂȂ�����J�n
    void OnEnable()
    {
        // �����_���ȓ����ƍU���̃R���[�`�����J�n
        StartCoroutine(nameof(RandomlyMovingCoroutine));
        StartCoroutine(nameof(RandomlyFireCoroutine));
    }

    void OnDisable()
    {
        // ���ׂẴR���[�`�����~
        StopAllCoroutines();
    }

    /// <summary>
    /// �����_���ȓ���������R���[�`��
    /// </summary>
    /// <returns></returns>
    IEnumerator RandomlyMovingCoroutine()
    {
        //�G�̈ʒu�������_���ɃZ�b�g
        transform.position = Viewport.Instance.RandomEnemySpawnPosition(paddingX, paddingY);

        //�ړ�������߂�
        Vector3 targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);

        //�I�u�W�F�N�g���A�N�e�B�u�ȏ�Ԃł���Ȃ�
        while(gameObject.activeSelf)
        {
            // �^�[�Q�b�g�ʒu�ɓ��B���Ă��Ȃ��ꍇ
            // ���������̃t���[���ł̈ړ����������������ꍇ�A�܂�A���̃t���[���ł̈ړ������łɖڕW�ʒu�ɔ��ɋ߂��ꍇ
            //else�ŐV�����ʒu���Z�b�g����
            if (Vector3.Distance(transform.position,targetPosition) >= moveSpeed * Time.fixedDeltaTime)//Mathf.Epsilon�@0�Ɍ���Ȃ��߂Â�float�^���ł�
            {
                // �^�[�Q�b�g�ʒu�Ɍ������Ĉړ�
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);

                // �G���ړ����Ă���Ƃ��� x ������]������
                //�^�[�Q�b�g��y���W�Ǝ��g��y���W���قȂ��
                //Vector3.right=>x���𒆐S�ɉ�]����@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@  normalized.y�̌��ʂɂ���č����������ł���Ώ�������A�����������ł���Ή��������悤�ɂȂ�܂�
                transform.rotation = Quaternion.AngleAxis((targetPosition - transform.position).normalized.y * moveRotationAngele, Vector3.right);
            }
            else
            {
                // �V�����^�[�Q�b�g�ʒu��ݒ�
                targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);
            }

            //�ړ��ɂ�����鉉�Z������FixedUpdate���g���Đ��x���グ��
            yield return waitForFixedUpdate;
        }
    }

    /// <summary>
    /// �U���Ԋu�ݒ�
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator RandomlyFireCoroutine()
    {
        while(gameObject.activeSelf)
        {
            // �����_���ȑҋ@���Ԃ�ݒ�
            yield return new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));

            // �Q�[�����Q�[���I�[�o�[��Ԃł���΁A�R���[�`�����I�� �܂�A�U�����Ȃ��悤�ɂ���
            if (GameManager.GameState == GameState.GameOver) yield break;

            // projectiles �z����̊e�e�̃v�[������̃����[�X
            foreach (var projectile in projectiles)
            {
                // �e���v�[�����烊���[�X���Amuzzle �̈ʒu�ɔz�u
                PoolManager.Release(projectile, muzzle.position);
            }

            // �����_���Ȓe���ˉ����Đ�
            AudioManager.Instance.PlayRandomSFX(projectileLaunchSFX);
        }
    }
}
