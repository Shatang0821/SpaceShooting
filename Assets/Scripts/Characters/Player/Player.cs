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

    //入力した移動方向
    Vector2 moveDirection;
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

    [SerializeField] AudioData dodgeSFX;//効果音

    [Tooltip("これはキャラクターのエネルギー消耗量です。")]
    [SerializeField,Range(0,100)] int dodgeEnergyCost = 25;

    [SerializeField] float maxRoll = 720f;//最大回転角度

    [SerializeField] float rollSpeed = 360f;//回転速度
    [SerializeField] Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);//スケール変化

    bool isDodging = false; //ダッジ　躱すかわす

    float currentRoll;//現在回転値

    float dodgeDuration;//ダッジ持続時間

    readonly float slowMotionDuration = 1f;//バレットタイムslowout時間
    #endregion

    #region Overdrive　
    //限界突破
    bool isOverdriving = false;

    [SerializeField] int overdriveDodgeFactor = 2;  //ダッジ消耗を２倍を増やす

    [SerializeField] float overdriveSpeedFactor = 1.2f;//スピードを1.2倍

    [SerializeField] float overdriveFireFactor = 1.2f;//攻撃間隔1.2倍縮む
    #endregion

    float t;                        //used for MoveCoroutine
    Vector2 previousVelocity;       //used for MoveCoroutine
    Quaternion previousRotation;    //used for MoveCoroutine


    WaitForSeconds waitForFireInterval;//攻撃間隔

    WaitForSeconds waitForOverdriveFireInterval;//オーバードライブの攻撃間隔

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
        // コンポーネントの取得と初期化
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        missile = GetComponent<MissileSystem>();

        //サイズ取得
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x / 2f;
        paddingY = size.y / 2f;

        waitForFireInterval = new WaitForSeconds(fireInterval);
        waitForOverdriveFireInterval = new WaitForSeconds(fireInterval /= overdriveFireFactor);
        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);
        waitDecelerationTime = new WaitForSeconds(decelerationTime);

        dodgeDuration = maxRoll / rollSpeed;//最大回転角度/回転速度 = 時間
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        //イベントのサブスクライブ
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
        //イベントのアンサブスクライブ
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
        statsBar_HUD.Initialize(health, maxHealth);

        rigidbody.gravityScale = 0f;//重力を0

        input.EnableGameplayInput();
    }

    /// <summary>
    /// ダメージを受ける処理
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public override void TakenDamage(float damage)
    {
        base.TakenDamage(damage);
        //バーを更新
        statsBar_HUD.UpdateStats(health, maxHealth);

        //アクティブ状態なっている時
        if (gameObject.activeSelf)
        {
            //弾とぶつかると止まるときがあり、それを防ぐために入力があるなら動かせる
            Move(moveDirection);
            //もし回復中であれば
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
        //バーを更新する
        statsBar_HUD.UpdateStats(health, maxHealth);
    }

    public override void Die()
    {
        //GameManager.onGameOverがnullでない場合のみInvokeを呼び出す
        //?はnull条件演算子
        GameManager.onGameOver?.Invoke();
        GameManager.GameState = GameState.GameOver;
        statsBar_HUD.UpdateStats(0f, maxHealth);
        base.Die();
    }

    #region MOVE
    void Move(Vector2 moveInput)
    {
        // Vector2 moveAmount = moveInput * moveSpeed;
        // rigidbody.velocity = moveAmount;
        //移動コルーチンがnullではない場合停止させる
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        //キーのvalueを正規化させて1か-1にする
        moveDirection = moveInput.normalized;
        //Quaternion moveRotation = Quaternion.AngleAxis(moveRotationAngle * moveInput.y,Vector3.right);
        moveCoroutine = StartCoroutine(MoveCoroutine(accelerationTime, moveDirection * moveSpeed, Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right)));
        //移動させるであれば減速コルーチンを止める
        StopCoroutine(nameof(DecelerationCoroutine));
        //画面制限を始まる
        StartCoroutine(nameof(MoveRangeLimatationCoroutine));
    }

    void StopMove()
    {
        //rigidbody.velocity = Vector2.zero;
        //移動コルーチンがnullではない場合停止させる
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        //速度と回転を減速時間によって徐々に初期に戻る
        moveCoroutine = StartCoroutine(MoveCoroutine(decelerationTime, Vector2.zero, Quaternion.identity));
        //減速時間を終わると画面制限を止める,,移動していないため画面外に出ることない
        StartCoroutine(nameof(DecelerationCoroutine));
    }
    /// <summary>
    ///  速度変更させる
    /// </summary>
    /// <param name="time">変更時間</param>
    /// <param name="moveVelocity">移動速度</param>
    /// <param name="moveRotation">回転情報</param>
    /// <returns></returns>
    IEnumerator MoveCoroutine(float time, Vector2 moveVelocity, Quaternion moveRotation)
    {
        t = 0f;
        previousVelocity = rigidbody.velocity;
        previousRotation = transform.rotation;

        while (t < 1f)
        {
            t += Time.fixedDeltaTime / time;
            //tが0から1の間にvelocityを補正させるtが1になると最大速度に達します
            rigidbody.velocity = Vector2.Lerp(previousVelocity, moveVelocity, t);
            //上と同じこと
            transform.rotation = Quaternion.Lerp(previousRotation, moveRotation, t);

            //物理演算させるからFixedUpdate使って精度を上げる
            yield return waitForFixedUpdate;
        }
    }

    /// <summary>
    /// 移動範囲を画面内に制限させる
    /// </summary>
    /// <returns></returns>
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

    //イベントを実行したらこれを呼び出す
    void Fire()
    {
        StartCoroutine(nameof(FireCoroutine));
    }
    //キーを話したら止まる
    void StopFire()
    {
        //StopCoroutine(FireCoroutine());//作用しない
        StopCoroutine(nameof(FireCoroutine));
    }

    /// <summary>
    //   攻撃コルーチン
    /// </summary>
    /// <returns></returns>
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
            //オーバードライブ状態ならその攻撃速度
            yield return isOverdriving ? waitForOverdriveFireInterval : waitForFireInterval;
            
        }
    }
    #endregion

    #region DODGE
    void Dodge()
    {
        //もしダッジ中であれば処理させないまた、エネルギーが足りないときも同様
        if (isDodging || !PlayerEnergy.Instance.IsEnough(dodgeEnergyCost)) return;
        //そうでなければ処理に入る
        StartCoroutine(nameof(DodgeCoroutine));
        // Change Player's scale
    }
    /// <summary>
    /// ダッジ実行コルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator DodgeCoroutine()
    {
        isDodging = true;
        AudioManager.Instance.PlayRandomSFX(dodgeSFX);
        // エネルギーを消耗
        PlayerEnergy.Instance.Use(dodgeEnergyCost);

        //物体と通過できるようにする
        collider.isTrigger = true;

        // 初期回転を0にする
        currentRoll = 0f;

        //バレットタイムを開始
        TimeController.Instance.BulletTime(slowMotionDuration, slowMotionDuration);

        //現在回転が最大回転より小さい場合ループさせる
        while (currentRoll < maxRoll)
        {
            currentRoll += rollSpeed * Time.deltaTime;
            //回転させる
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
            //これ意味不明の参考ベジェ曲線を使って動きをなめらかにする
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
        //エネルギーが足りない場合処理させない
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
