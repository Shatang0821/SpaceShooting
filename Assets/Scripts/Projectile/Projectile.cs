using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject hitVFX;
    [SerializeField] AudioData[] hitSFX;

    [SerializeField] float damage;

    [SerializeField] protected float moveSpeed = 10f;      //弾の初期速度

    [SerializeField] protected Vector2 moveDirection;      //弾の移動方向

    protected GameObject target;

    protected virtual void OnEnable()                     //この関数はオブジェクトが有効
                                                //アクティブになったときに呼び出されます
    {
        StartCoroutine(MoveDirectly());
    }

   

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<Character>(out Character character))
        {
            character.TakenDamage(damage);
            //var contactPoint = collision.GetContact(0);
            //PoolManager.Release(hitVFX, contactPoint.point, Quaternion.LookRotation(contactPoint.normal));
            PoolManager.Release(hitVFX, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal)); ;
            AudioManager.Instance.PlayRandomSFX(hitSFX);
            gameObject.SetActive(false);
        }
    }
    
    IEnumerator MoveDirectly()
    {
        while(gameObject.activeSelf)
        {
            Move();
            yield return null;
        }
    }

    protected void SetTarget(GameObject target) => this.target = target;

    //オブジェクトのローカル前方向を基づいて移動する。ローカル方向のz軸は前を意味するため,z軸変えると進行方向も変わる
    public void Move() => transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
}
