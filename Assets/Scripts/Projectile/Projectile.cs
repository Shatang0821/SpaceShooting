using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]float moveSpeed = 10f;      //�e�̏������x

    [SerializeField] protected Vector2 moveDirection;      //�e�̈ړ�����

    protected GameObject target;

    protected virtual void OnEnable()                     //���̊֐��̓I�u�W�F�N�g���L��
                                                //�A�N�e�B�u�ɂȂ����Ƃ��ɌĂяo����܂�
    {
        StartCoroutine(MoveDirectly());
    }

    IEnumerator MoveDirectly()
    {
        while(gameObject.activeSelf)
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

            yield return null;
        }
    }
}
