using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldCharacter : MonoBehaviour
{
    [Header("---- DEATH ----")]
    //死亡効果
    [SerializeField] GameObject deathVFX;

    [SerializeField] AudioData[] deathSFX;

    [Header("---- HEALTH ----")]
    [SerializeField] protected float maxHealth;

    [SerializeField] protected float health;

    [SerializeField] bool showOnHeadHealthBar = true;

    [SerializeField] StatsBar onHeadHealtherBar;

    //protected 継承した先でもこれをアクセスできる
    //virtual オーバーライトできる
    protected virtual void OnEnable()
    {
        health = maxHealth;

        if(showOnHeadHealthBar)
        {
            ShowOnHeadHealthBar();
        }
        else
        {
            HideOnHeadHealthBar();
        }
    }

    public void ShowOnHeadHealthBar()
    {
        onHeadHealtherBar.gameObject.SetActive(true);
        onHeadHealtherBar.Initialize(health, maxHealth);
    }

    public void HideOnHeadHealthBar()
    {
        onHeadHealtherBar.gameObject.SetActive(false);
    }

    public virtual void TakenDamage(float damage)
    {
        if (health == 0f) return;
        health -= damage;

        if (showOnHeadHealthBar)
        {
            onHeadHealtherBar.UpdateStats(health, maxHealth);
        }
            

        if (health <= 0f)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        //UI上でHPを０させるため
        health = 0f;
        AudioManager.Instance.PlayRandomSFX(deathSFX);
        PoolManager.Release(deathVFX,transform.position);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// HPを回復させる
    /// </summary>
    /// <param name="value">回復量</param>
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
        if(showOnHeadHealthBar)
        {
            onHeadHealtherBar.UpdateStats(health, maxHealth);
        }
    }

    /// <summary>
    /// 一定時間を待ってから回復させる
    /// </summary>
    /// <param name="waitTime">待つ時間</param>
    /// <param name="percent">回復パーセント</param>
    /// <returns></returns>
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
