using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{
    TrailRenderer trail;

    private void Awake()
    {
        trail = GetComponentInChildren<TrailRenderer>();

        if (moveDirection != Vector2.right)
        {
            transform.rotation = Quaternion.FromToRotation(Vector2.left, moveDirection);//開始方向から終了方向まで回転数値
        }
    }

    private void OnDisable()
    {
        trail.Clear();
    }
}
