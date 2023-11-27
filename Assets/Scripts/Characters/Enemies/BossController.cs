using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BossController : EnemyController
{
    [SerializeField] float continuousFireDuration = 1.5f;

    [Header("---- Player Detection ----")]

    [SerializeField] Transform playerDetectionTransform;

    [SerializeField] Vector3 playerDetectionSize;

    [SerializeField] LayerMask playerLayer;

    WaitForSeconds waitForContinuousFireInterval;

    WaitForSeconds waitForFireInterval;

    List<GameObject> magazine;

    AudioData launchSFX;

    protected override void Awake()
    {
        base.Awake();
        waitForContinuousFireInterval = new WaitForSeconds(minFireInterval);
        waitForFireInterval = new WaitForSeconds(maxFireInterval);

        magazine = new List<GameObject>(projectiles.Length);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(playerDetectionTransform.position, playerDetectionSize);
    }

    void LoadProjectiles()
    {
        magazine.Clear();

        if (Physics2D.OverlapBox(playerDetectionTransform.position,playerDetectionSize,0f,playerLayer))
        {
            //Launch projectile 1
            magazine.Add(projectiles[0]);
            launchSFX = projectileLaunchSFX[0];
        }
        else
        {
            //Launch projectile 2 or 3
            if(Random.value < 0.5f)
            {
                magazine.Add(projectiles[1]);
                launchSFX = projectileLaunchSFX[1];
            }
            else
            {
                for(int i = 2;i<projectiles.Length;i++)
                {
                    magazine.Add(projectiles[i]);
                }

                launchSFX = projectileLaunchSFX[2];
            }
        }
    }

    IEnumerator CoutinuousFireCoroutine()
    {
        LoadProjectiles();

        float continuousFireTimer = 0f;
        while(continuousFireTimer < continuousFireDuration)
        {
            foreach(var projectile in magazine)
            {
                PoolManager.Release(projectile, muzzle.position);
            }

            continuousFireTimer += minFireInterval;
            AudioManager.Instance.PlayRandomSFX(launchSFX);

            yield return waitForContinuousFireInterval;
        }
    }

    protected override IEnumerator RandomlyFireCoroutine()
    {
        if (GameManager.GameState == GameState.GameOver) yield break;

        while(isActiveAndEnabled)
        {
            yield return waitForFireInterval;
            yield return StartCoroutine(nameof(CoutinuousFireCoroutine));
        }
    }
}
