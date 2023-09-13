using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Player : Character
{
    [SerializeField] StatsBar_HUD statsBar_HUD;

    //回復スイッチ
    [SerializeField] bool regenerateHealth = true;

    //回復までの時間
    [SerializeField] float healthRegenerateTime;

    //回復パーセント
    [SerializeField,Range(0f, 1f)] float healthRegeneratePercent;

    #region PlayerAttributes

    [Header("---- INPUT ----")]
    [SerializeField] PlayerInput input;

    [Header("---- PlayerAttributes ----")]
    
    [Tooltip("これはキャラクターの最大速度です。")]
    [SerializeField] float moveSpeed = 10f;

    [Tooltip("これはキャラクターの加速時間です。")]
    [SerializeField] float accelerationTime = 3f;   //加速時間

    [Tooltip("これはキャラクターの減速時間です。")]
    [SerializeField] float decelerationTime = 3f;   //減速時間

    [Tooltip("これはキャラクターの上下移動角度です。")]
    [SerializeField] float moveRotationAngle = 50f;

    [Tooltip("これはキャラクターのｘ端の数値です。")]
    [SerializeField] float paddingx = 0.2f;

    [Tooltip("これはキャラクターのｙ端の数値です。")]
    [SerializeField] float paddingy = 0.2f;
    #endregion

    #region ProjectileAttributes

    [Header("---- ProjectileAttributes ----")]
    [Tooltip("これはキャラクターの弾オブジェクトです。")]
    [SerializeField] GameObject projectile1; //弾オブジェクト
    [SerializeField] GameObject projectile2; //弾オブジェクト
    [SerializeField] GameObject projectile3; //弾オブジェクト

    [Tooltip("これはキャラクターの弾発射位置です。")]
    [SerializeField] Transform muzzleMiddle;      //弾発射位置
    [SerializeField] Transform muzzleTop;      //弾発射位置
    [SerializeField] Transform muzzleBottom;      //弾発射位置

    [Tooltip("これはキャラクターのパワーです。")]
    [SerializeField, Range(0, 2)] int weaponPower = 0;

    [Tooltip("これはキャラクターの弾発射間隔です。")]
    [SerializeField] float fireInterval = 0.2f;    //弾発射間隔
    
    #endregion

    WaitForSeconds waitForFireInterval;

    //HP自動回復時間
    WaitForSeconds waitHealthRegenerateTime;

    new Rigidbody2D rigidbody;

    Coroutine moveCoroutine;

    //HealthRegenerateCoroutineを中止するための入れ物
    Coroutine healthRegenerateCoroutine;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

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
        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);

        statsBar_HUD.Initialize(health, maxHealth);

        rigidbody.gravityScale = 0f;//重力を0

        input.EnableGameplayInput();

    }


    public override void TakenDamage(float damage)
    {
        base.TakenDamage(damage);
        statsBar_HUD.UpdateStats(health, maxHealth);

        //アクティブ状態なっている時
        if(gameObject.activeSelf)
        {
            if (regenerateHealth)
            {
                //もしコルーチンが始まった時でもダメージを受けるとリセットさせます
                if(healthRegenerateCoroutine != null)
                {
                    StopCoroutine(healthRegenerateCoroutine);
                }
               healthRegenerateCoroutine = StartCoroutine(HealthRegenerateCoroutine(waitHealthRegenerateTime, healthRegeneratePercent));
            }
        }
    }

    public override void RestoreHealth(float value)
    {
        base.RestoreHealth(value);
        statsBar_HUD.UpdateStats(health, maxHealth);
    }

    public override void Die()
    {
        statsBar_HUD.UpdateStats(0f, maxHealth);
        base.Die();
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

        while(t < 1f)
        {
            t += Time.fixedDeltaTime / time;
            rigidbody.velocity = Vector2.Lerp(rigidbody.velocity, moveVelocity, t);
            transform.rotation = Quaternion.Lerp(transform.rotation, moveRotation, t);

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
            switch (weaponPower)
            {
                case 0:
                    PoolManager.Release(projectile1, muzzleMiddle.position);//弾を生成する
                    break;
                case 1:
                    PoolManager.Release(projectile1, muzzleTop.position);//弾を生成する
                    PoolManager.Release(projectile1, muzzleBottom.position);//弾を生成する
                    break;
                case 2:
                    PoolManager.Release(projectile1, muzzleMiddle.position);//弾を生成する
                    PoolManager.Release(projectile2, muzzleTop.position);//弾を生成する
                    PoolManager.Release(projectile3, muzzleBottom.position);//弾を生成する
                    break;
                default:
                    break;

            }

            yield return waitForFireInterval;
        }
    }
    #endregion
}
