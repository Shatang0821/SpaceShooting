using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField]PlayerInput input;

    [SerializeField] float moveSpeed = 10f;

    [SerializeField] float accelerationTime = 3f;   //加速時間

    [SerializeField] float decelerationTime = 3f;   //減速時間

    [SerializeField] float moveRotationAngle = 50f;

    [SerializeField] float paddingx = 0.2f;

    [SerializeField] float paddingy = 0.2f;

    [SerializeField] GameObject projectile; //弾オブジェクト

    [SerializeField] Transform muzzle;      //弾発射位置
    [SerializeField] float fireInterval = 0.2f;    //弾発射間隔


    WaitForSeconds waitForFireInterval;

    new Rigidbody2D rigidbody;

    Coroutine moveCoroutine;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        input.onMove += Move;
        input.onStopMove += StopMove;

        input.onFire += Fire;
        input.onStopFire += StopFire;
    }

    void OnDisable()
    {
        input.onMove -= Move;
        input.onStopMove -= StopMove;

        input.onFire -= Fire;
        input.onStopFire -= StopFire;
    }
    // Start is called before the first frame update
    void Start()
    {
        //もしこれからこの発射間隔をゲーム内に編集するなら関数を作って間隔を変えるときに
        //以下の文を関数内に置くことにより、毎回編集するときが新しく作ります。
        waitForFireInterval = new WaitForSeconds(fireInterval);

        rigidbody.gravityScale = 0f;//重力を0

        input.EnableGameplayInput();
    }

    #region MOVE
    void Move(Vector2 moveInput)
    {
        // Vector2 moveAmount = moveInput * moveSpeed;
        // rigidbody.velocity = moveAmount;
        if(moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        //Quaternion moveRotation = Quaternion.AngleAxis(moveRotationAngle * moveInput.y,Vector3.right);
        moveCoroutine =  StartCoroutine(MoveCoroutine(accelerationTime,moveInput.normalized * moveSpeed, Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right)));
        StartCoroutine(MovePositionLimitCoroutine());
    }

    void StopMove()
    {
        //rigidbody.velocity = Vector2.zero;
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveCoroutine(decelerationTime,Vector2.zero,Quaternion.identity));
        StopCoroutine(MovePositionLimitCoroutine());
    }

    IEnumerator MoveCoroutine(float time ,Vector2 moveVelocity,Quaternion moveRotation)
    {
        float t = 0f;

        while(t < time)
        {
            t += Time.fixedDeltaTime / decelerationTime;
            rigidbody.velocity = Vector2.Lerp(rigidbody.velocity, moveVelocity, t / time);
            transform.rotation = Quaternion.Lerp(transform.rotation, moveRotation, t / time);

            yield return null;
        }
    }

 

    IEnumerator MovePositionLimitCoroutine()
    {
        while (true)
        {
            transform.position = Viewport.Instance.PlayerMoveablePosition(transform.position,paddingx,paddingy);

            yield return null;
        }
       
    }
    #endregion

    #region FIRE
    
    void Fire()
    {
        StartCoroutine(nameof(FireCoroutine));
    }

    void StopFire()
    {
        //StopCoroutine(FireCoroutine());//作用しない
        StopCoroutine(nameof(FireCoroutine));
    }

    IEnumerator FireCoroutine()
    {
        while (true)
        {
            Instantiate(projectile, muzzle.position, Quaternion.identity);  //弾を生成する

            yield return waitForFireInterval;
        }
    }
    #endregion
}
