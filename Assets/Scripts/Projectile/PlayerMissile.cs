using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : PlayerProjectileOverdrive
{
    [SerializeField] AudioData targetAcquiredVoice = null;

    [Header("==== SPEED CHANGE ====")]
    [SerializeField] float lowSpeed = 8f;   //遅いスピード
    [SerializeField] float highSpeed = 25f; //速いスピード
    [SerializeField] float variableSpeedDelay = 0.5f;   //スピード変化遅延

    [Header("==== EXPLOSION ====")]
    [SerializeField] GameObject explosionVFX = null;
    [SerializeField] AudioData explosionSFX = null;

    [SerializeField] LayerMask enemyLayerMask = default;

    [SerializeField] float explosionRadius = 3f;
    [SerializeField] float explosionDamage = 100f;

    WaitForSeconds waitVariableSpeedDelay;

    protected override void Awake()
    {
        base.Awake();
        waitVariableSpeedDelay = new WaitForSeconds(variableSpeedDelay);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(nameof(VariableSpeedCoroutine));
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        //爆発エフェクト
        PoolManager.Release(explosionVFX, transform.position);
        //爆発サウンド
        AudioManager.Instance.PlayRandomSFX(explosionSFX);
        //AOEダメージ
        //円範囲を描く、すべてのエネミー返す
        var colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius,enemyLayerMask);

        foreach(var collider in colliders)
        {
            if(collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakenDamage(explosionDamage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);    
    }

    IEnumerator VariableSpeedCoroutine()
    {
        moveSpeed = lowSpeed;

        yield return waitVariableSpeedDelay;

        moveSpeed = highSpeed;

        if(target != null)
        {
            AudioManager.Instance.PlayRandomSFX(targetAcquiredVoice);
        }
    }
}
