using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{
    TrailRenderer trail;

    protected virtual void Awake()
    {
        trail = GetComponentInChildren<TrailRenderer>();

        if (moveDirection != Vector2.right)
        {
            transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector2.right, moveDirection);//開始方向から終了方向まで回転数値
        }
    }

    private void OnDisable()
    {
        trail.Clear();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        //base.OnCollisionEnter2D(collision);
        if(collision.gameObject.tag == "Enemy")
        {
            TEST_EnemyManager.Instance.Damage(collision.gameObject, 10);
            PoolManager.Release(hitVFX, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal));
            AudioManager.Instance.PlayRandomSFX(hitSFX);
            gameObject.SetActive(false);
        }
        PlayerEnergy.Instance.Obtain(PlayerEnergy.PERCENT);
    }


}
