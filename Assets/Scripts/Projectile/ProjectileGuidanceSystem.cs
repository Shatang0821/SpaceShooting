using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGuidanceSystem : MonoBehaviour
{
    [SerializeField] Projectile projectile;

    [SerializeField] float minBallisticAngle = 50f;
    [SerializeField] float maxBallisticAngle = 75f;

    float ballisticAngle;

    Vector3 targetDirection;
    public IEnumerator HomingCoroutine(GameObject target)
    {
        ballisticAngle = Random.Range(minBallisticAngle, maxBallisticAngle);
        while(gameObject.activeSelf)
        {
            if(target.activeSelf)
            {
                targetDirection = target.transform.position - transform.position;

                // rotate to target
                //var angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg, Vector3.forward);
                //角度を少しずつ補正していく効果
                transform.rotation *= Quaternion.Euler(0f, 0f, ballisticAngle);
                projectile.Move();
            }
            else
            {
                projectile.Move();
            }

            yield return null;
        }
    }
}
