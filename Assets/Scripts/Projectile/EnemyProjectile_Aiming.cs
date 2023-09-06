using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile_Aiming : Projectile
{
    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void OnEnable()
    {
        StartCoroutine(nameof(MoveDirectionCoroutine));
        base.OnEnable();

    }
    //1�t���[����҂��āA���m�Ȓl��^����悤�ɂ��邽��
    IEnumerator MoveDirectionCoroutine()
    {
        Debug.Log("Target Position: " + target.transform.position);
        Debug.Log("Current Position: " + transform.position);
        Debug.Log("Move Direction: " + moveDirection);
        yield return null;

        if (target.activeSelf)
        {
            moveDirection = (target.transform.position - transform.position).normalized;
        }
    }
}
