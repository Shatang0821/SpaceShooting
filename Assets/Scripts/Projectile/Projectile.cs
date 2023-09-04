using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]float moveSpeed = 10f;      //弾の初期速度

    [SerializeField] Vector2 moveDirection;      //弾の移動方向

    private void OnEnable()                     //この関数はオブジェクトが有効
                                                //アクティブになったときに呼び出されます
    {
        StartCoroutine(MoveDirectly());
    }

    IEnumerator MoveDirectly()
    {
        while(gameObject.activeSelf)
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

            yield return null;
        }
    }
}
