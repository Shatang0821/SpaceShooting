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
    [SerializeField, Range(0f, 1f)] float healthRegeneratePercent;

    #region PlayerAttributes

    [Header("---- INPUT ----")]
    [SerializeField] PlayerInput input;

    [Header("---- MOVE ----")]

    [Tooltip("これはキャラクターの最大速度です。")]
    [SerializeField] float moveSpeed = 10f;

    [Tooltip("これはキャラクターの加速時間です。")]
    [SerializeField] float accelerationTime = 3f;   //加速時間

    [Tooltip("これはキャラクターの減速時間です。")]
    [SerializeField] float decelerationTime = 3f;   //減速時間

    [Tooltip("これはキャラクターの上下移動角度です。")]
    [SerializeField] float moveRotationAngle = 50f;

    float paddingX;
    float paddingY;
    #endregion

    #region ProjectileAttributes

    [Header("---- FIRE ----")]
    [Tooltip("これはキャラクターの弾オブジェクトです。")]
    [SerializeField] GameObject projectile1; //弾オブジェクト
    [SerializeField] GameObject projectile2; //弾オブジェクト
    [SerializeField] GameObject projectile3; //弾オブジェクト
    [SerializeField] GameObject projectileOverdrive;

    [Tooltip("これはキャラクターの弾発射位置です。")]
    [SerializeField] Transform muzzleMiddle;      //弾発射位置
    [SerializeField] Transform muzzleTop;      //弾発射位置
    [SerializeField] Transform muzzleBottom;      //弾発射位置

    [SerializeField] AudioData projectileLaunchSFX;   //発射効果音

    [Tooltip("これはキャラクターのパワーです。")]
    [SerializeField, Range(0, 2)] int weaponPower = 0;

    [Tooltip("これはキャラクターの弾発射間隔です。")]
    [SerializeField] float fireInterval = 0.2f;    //弾発射間隔



    #endregion

    #region PlayerDodge
    [Header("----- DODGE -----")]

    [SerializeField] AudioData dodgeSFX;

    [Tooltip("これはキャラクターのエネルギー消耗量です。")]
    [SerializeField,Range(0,100)] int dodgeEnergyCost = 25;

    [SerializeField] float maxRoll = 720f;

    [SerializeField] float rollSpeed = 360f;
    [SerializeField] Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);

    bool isDodging = false;

    float currentRoll;

    float dodgeDuration;

    readonly float slowMotionDuration = 1f;//バレットタイムslowout時間
    #endregion

    #region Overdrive

    bool isOverdriving = false;

    [SerializeField] int overdriveDodgeFactor = 2;

    [SerializeField] float overdriveSpeedFactor = 1.2f;

    [SerializeField] float overdriveFireFactor = 1.2f;
    #endregion

    float t;                        //used for MoveCoroutine
    Vector2 previousVelocity;       //used for MoveCoroutine
    Quaternion previousRotation;    //used for MoveCoroutine


    WaitForSeconds waitForFireInterval;

    WaitForSeconds waitForOverdriveFireInterval;

    //HP自動回復時間
    WaitForSeconds waitHealthRegenerateTime;

    WaitForSeconds waitDecelerationTime;//減速時間

    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();   //used for MoveCoroutine

    new Rigidbody2D rigidbody;

    new Collider2D collider;

    MissileSystem missile;

    Coroutine moveCoroutine;
    //HealthRegenerateCoroutineを中止するための入れ物
    Coroutine healthRegenerateCoroutine;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        missile = GetComponent<MissileSystem>();

        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x / 2f;
        paddingY = size.y / 2f;

        waitForFireInterval = new WaitForSeconds(fireInterval);
        waitForOverdriveFireInterval = new WaitForSeconds(fireInterval /= overdriveFireFactor);
        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);
        waitDecelerationTime = new WaitForSeconds(decelerationTime);

        dodgeDuration = maxRoll / rollSpeed;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        input.onMove += Move;
        input.onStopMove += StopMove;
        input.onFire += Fire;
        input.onStopFire += StopFire;
        input.onDodge += Dodge;
        input.onOverdrive += Overdrive;
        input.onLaunchMissile += LaunchMissile;

        PlayerOverdrive.on += OverdriveOn;
        PlayerOverdrive.off += OverdriveOff;
    }

    void OnDisable()
    {
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        input.onFire -= Fire;
        input.onStopFire -= StopFire;
        input.onDodge -= Dodge;
        input.onOverdrive -= Overdrive;
        input.onLaunchMissile -= LaunchMissile;

        PlayerOverdrive.on -= OverdriveOn;
        PlayerOverdrive.off -= OverdriveOff;
    }
    // Start is called before the first frame update
    void Start()
    {
        //もしこれからこの発射間隔をゲーム内に編集するなら関数を作って間隔を変えるときに
        //以下の文を関数内に置くことにより、毎回編集するときが新しく作ります。
        statsBar_HUD.Initialize(health, maxHealth);

        rigidbody.gravityScale = 0f;//重力を0

        input.EnableGameplayInput();
    }


    public override void TakenDamage(float damage)
    {
        base.TakenDamage(damage);
        statsBar_HUD.UpdateStats(health, maxHealth);

        //アクティブ状態なっている時
        if (gameObject.activeSelf)
        {
            if (regenerateHealth)
            {
                //もしコルーチンが始まった時でもダメージを受けるとリセットさせます
                if (healthRegenerateCoroutine != null)
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
        GameManager.GameState = GameState.GameOver;
        statsBar_HUD.UpdateStats(0f, maxHealth);
        base.Die();
    }

    #region MOVE
    void Move(Vector2 moveInput)
    {
        // Vector2 moveAmount = moveInput * moveSpeed;
        // rigidbody.velocity = moveAmount;
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        //Quaternion moveRotation = Quaternion.AngleAxis(moveRotationAngle * moveInput.y,Vector3.right);
        moveCoroutine = StartCoroutine(MoveCoroutine(accelerationTime, moveInput.normalized * moveSpeed, Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right)));
        StopCoroutine(nameof(DecelerationCoroutine));
        StartCoroutine(nameof(MoveRangeLimatationCoroutine));
    }

    void StopMove()
    {
        //rigidbody.velocity = Vector2.zero;
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveCoroutine(decelerationTime, Vector2.zero, Quaternion.identity));
        StartCoroutine(nameof(DecelerationCoroutine));
    }

    IEnumerator MoveCoroutine(float time, Vector2 moveVelocity, Quaternion moveRotation)
    {
        t = 0f;
        previousVelocity = rigidbody.velocity;
        previousRotation = transform.rotation;

        while (t < 1f)
        {
            t += Time.fixedDeltaTime / time;
            rigidbody.velocity = Vector2.Lerp(previousVelocity, moveVelocity, t);
            transform.rotation = Quaternion.Lerp(previousRotation, moveRotation, t);

            yield return waitForFixedUpdate;
        }
    }

    IEnumerator MoveRangeLimatationCoroutine()
    {
        while(true)
        {
            transform.position = Viewport.Instance.PlayerMoveablePosition(transform.position, paddingX, paddingY);
            yield return null;
        }    
    }

    IEnumerator DecelerationCoroutine()
    {
        yield return waitDecelerationTime;

        StopCoroutine(MoveRangeLimatationCoroutine());
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
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleMiddle.position);//弾を生成する
                    break;
                case 1:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleTop.position);//弾を生成する
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleBottom.position);//弾を生成する
                    break;
                case 2:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleMiddle.position);//弾を生成する
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile2, muzzleTop.position);//弾を生成する
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile3, muzzleBottom.position);//弾を生成する
                    break;
                default:
                    break;

            }

            AudioManager.Instance.PlayRandomSFX(projectileLaunchSFX);
            yield return isOverdriving ? waitForOverdriveFireInterval : waitForFireInterval;
            
        }
    }
    #endregion

    #region DODGE
    void Dodge()
    {
        if (isDodging || !PlayerEnergy.Instance.IsEnough(dodgeEnergyCost)) return;
        StartCoroutine(nameof(DodgeCoroutine));
        // Change Player's scale
    }

    IEnumerator DodgeCoroutine()
    {
        isDodging = true;
        AudioManager.Instance.PlayRandomSFX(dodgeSFX);
        // Cost energy
        PlayerEnergy.Instance.Use(dodgeEnergyCost);

        //Make player invincibal
        collider.isTrigger = true;

        // Make player rotate along x axis
        currentRoll = 0f;

        //var scale = transform.localScale;

        TimeController.Instance.BulletTime(slowMotionDuration, slowMotionDuration);

        while (currentRoll < maxRoll)
        {
            currentRoll += rollSpeed * Time.deltaTime;
            transform.rotation = Quaternion.AngleAxis(currentRoll, Vector3.right);

            /*
             if (currentRoll < maxRoll / 2f)
            {
                scale.x = Mathf.Clamp(scale.x - Time.deltaTime / dodgeDuration,dodgeScale.x,1f);
                scale.y = Mathf.Clamp(scale.y - Time.deltaTime / dodgeDuration, dodgeScale.y, 1f);
                scale.z = Mathf.Clamp(scale.z - Time.deltaTime / dodgeDuration, dodgeScale.z, 1f);
            }
            else
            {
                scale.x = Mathf.Clamp(scale.x + Time.deltaTime / dodgeDuration, dodgeScale.x, 1f);
                scale.y = Mathf.Clamp(scale.y + Time.deltaTime / dodgeDuration, dodgeScale.y, 1f);
                scale.z = Mathf.Clamp(scale.z + Time.deltaTime / dodgeDuration, dodgeScale.z, 1f);
            }
             */

            transform.localScale = BezierCurve.QuadraticPoint(Vector3.one, Vector3.one, dodgeScale, currentRoll / maxRoll);
            yield return null;
        }

        collider.isTrigger = false;
        isDodging = false;
    }
    #endregion

    #region OVERDRIVE
    void Overdrive()
    {
        Debug.Log("yes");
        if (!PlayerEnergy.Instance.IsEnough(PlayerEnergy.MAX)) return;

        PlayerOverdrive.on.Invoke();
    }

    void OverdriveOn()
    {
        
        isOverdriving = true;
        dodgeEnergyCost *= overdriveDodgeFactor;
        moveSpeed *= overdriveSpeedFactor;
    }

    void OverdriveOff()
    {
        isOverdriving = false;
        dodgeEnergyCost /= overdriveDodgeFactor;
        moveSpeed /= overdriveSpeedFactor;
    }
    #endregion

    void LaunchMissile()
    {
        missile.Launch(muzzleMiddle);
    }
}
