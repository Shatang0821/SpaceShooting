using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected GameObject hitVFX; //�q�b�g�G�t�F�N�g
    [SerializeField] protected AudioData[] hitSFX;//�q�b�g�T�E���h

    [SerializeField] float damage;  //�_���[�W

    [SerializeField] protected float moveSpeed = 10f;      //�e�̏������x

    [SerializeField] protected Vector2 moveDirection;      //�e�̈ړ�����

    protected GameObject target;    //�ǔ��e�Ȃ�g��

    protected virtual void OnEnable()                     //���̊֐��̓I�u�W�F�N�g���L��
                                                //�A�N�e�B�u�ɂȂ����Ƃ��ɌĂяo����܂�
    {
        StartCoroutine(MoveDirectly());
    }

   

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<OldCharacter>(out OldCharacter character))
        {
            character.TakenDamage(damage);
            //�Փ˂̍ŏ��̐ڐG�_�̖@�������ɉ�]
            PoolManager.Release(hitVFX, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal));
            AudioManager.Instance.PlayRandomSFX(hitSFX);
            gameObject.SetActive(false);
        }
    }
    
    IEnumerator MoveDirectly()
    {
        while(gameObject.activeSelf)
        {
            Move();
            yield return null;
        }
    }

    protected void SetTarget(GameObject target) => this.target = target;

    //�I�u�W�F�N�g�̃��[�J���O��������Â��Ĉړ�����B���[�J��������z���͑O���Ӗ����邽��,z���ς���Ɛi�s�������ς��
    public void Move() => transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
}
