using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //���S����
    [SerializeField] GameObject deathVFX;

    [Header("---- HEALTH ----")]
    [SerializeField] protected float maxHealth;

    [SerializeField] protected float health;

    //protected �p��������ł�������A�N�Z�X�ł���
    //virtual �I�[�o�[���C�g�ł���
    protected virtual void OnEnable()
    {
        health = maxHealth;
    }

    public virtual void TakenDamage(float damage)
    {
        health -= damage;

        if (health <= 0f)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        //UI���HP���O�����邽��
        health = 0f;

        PoolManager.Release(deathVFX,transform.position);
        gameObject.SetActive(false);
    }

    public virtual void RestoreHealth(float value)
    {
        if(health<=maxHealth)
        {
            //health += value;
            //health = Mathf.Clamp(health, 0f, maxHealth);

            //���̋�Ԃɉz���Ȃ�����
            //health + value ��0����maxHealth�̋�Ԃɂ��邱��
            health = Mathf.Clamp(health + value, 0f, maxHealth);
        }
    }

    //�p�[�Z���g��
    protected IEnumerator HealthRegenerateCoroutine(WaitForSeconds waitTime,float percent)
    {
        while(health<maxHealth)
        {
            yield return waitTime;

            RestoreHealth(maxHealth * percent);
        }
    }

    //�����_���[�W
    protected IEnumerator DamegeOverTimeCoroutine(WaitForSeconds waitTime, float percent)
    {
        while (health > 0f)
        {
            yield return waitTime;

            TakenDamage(maxHealth * percent);
        }
    }
}
