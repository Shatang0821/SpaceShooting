using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyProjectile : Projectile
{
    void Awake()
    {
        if (moveDirection != Vector2.left)
        {
            transform.rotation = Quaternion.FromToRotation(Vector2.left, moveDirection);//開始方向から終了方向まで回転数値
        }
    }
}
