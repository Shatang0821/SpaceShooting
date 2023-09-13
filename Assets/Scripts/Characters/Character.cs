using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //死亡効果
    [SerializeField] GameObject deathVFX;

    [Header("---- HEALTH ----")]
    [SerializeField] protected float maxHealth;

    [SerializeField] protected float health;

    //protected 継承した先でもこれをアクセスできる
    //virtual オーバーライトできる
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
        //UI上でHPを０させるため
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

            //この区間に越えないこと
            //health + value が0からmaxHealthの区間にあること
            health = Mathf.Clamp(health + value, 0f, maxHealth);
        }
    }

    //パーセント回復
    protected IEnumerator HealthRegenerateCoroutine(WaitForSeconds waitTime,float percent)
    {
        while(health<maxHealth)
        {
            yield return waitTime;

            RestoreHealth(maxHealth * percent);
        }
    }

    //持続ダメージ
    protected IEnumerator DamegeOverTimeCoroutine(WaitForSeconds waitTime, float percent)
    {
        while (health > 0f)
        {
            yield return waitTime;

            TakenDamage(maxHealth * percent);
        }
    }
}
