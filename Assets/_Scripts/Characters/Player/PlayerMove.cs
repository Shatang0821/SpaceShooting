using Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("---- MOVE ----")]
    [Tooltip("キャラクターの最大速度です。")]
    [SerializeField] float moveSpeed = 10f;

    [Tooltip("キャラクターの特殊状態スピード倍数")]
    [SerializeField] float overdriveSpeedFactor = 1.2f;//スピードを1.2倍

    [Tooltip("キャラクターの加速時間です。")]
    [SerializeField] float accelerationTime = 3f;   //加速時間

    [Tooltip("キャラクターの減速時間です。")]
    [SerializeField] float decelerationTime = 3f;   //減速時間

    [Tooltip("キャラクターの上下回転角度です。")]
    [SerializeField] float moveRotationAngle = 50f;

    //プレイヤー情報
    private Player _player;

    private PlayerInput _input;

    private new Rigidbody2D rigidbody;

    private OptionManager optionManager;

    //モデルのXYの半分サイズ
    private float paddingX;
    private float paddingY;

    //入力した移動方向を保持する変数
    Vector2 moveDirection;
    /*
        事前に定義しておくによって繰り返し定義を防ぐ
     */
    float t;                        //used for MoveCoroutine
    Vector2 previousVelocity;       //used for MoveCoroutine
    Quaternion previousRotation;    //used for MoveCoroutine

    //コルーチン関連変数
    WaitForSeconds waitDecelerationTime;//減速時間

    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();//FixedUpdate物理演算最適

    Coroutine moveCoroutine;//移動コルーチンの重複開始を防ぐための入れもの

    Coroutine optionMoveCoroutine;

    /// <summary>
    /// 初期化
    /// </summary>
    void Initialized()
    {
        _player = GetComponent<Player>();
        rigidbody = GetComponent<Rigidbody2D>();
        optionManager = GetComponentInChildren<OptionManager>();
        waitDecelerationTime = new WaitForSeconds(decelerationTime);
    }

    /// <summary>
    /// 機体サイズ取得
    /// </summary>
    void SetSize()
    {
        //サイズ取得
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x / 2f;
        paddingY = size.y / 2f;
    }

    private void Awake()
    {
        Initialized();
        
        SetSize();
    }

    private void Start()
    {
        _input =_player.input;

        rigidbody.gravityScale = 0f;//重力を0
    }

    private void OnEnable()
    {
        EventCenter.Subscribe(EventKeyManager.Move, Move);
        EventCenter.Subscribe(EventKeyManager.StopMove, StopMove);

        EventCenter.Subscribe(EventKeyManager.PlayerOverDriveOn, OverDriveOn);

        EventCenter.Subscribe(EventKeyManager.OverDriveOff, OverDriveOff);

        EventCenter.Unsubscribe(EventKeyManager.PlayerOverDriveOn, OverDriveOn);

        EventCenter.Unsubscribe(EventKeyManager.OverDriveOff, OverDriveOff);
    }

    private void OnDisable()
    {
        EventCenter.Unsubscribe(EventKeyManager.Move, Move);
        EventCenter.Unsubscribe(EventKeyManager.StopMove, StopMove);
    }

    /// <summary>
    /// 移動処理<br/>
    /// コルーチンを始まる処理<br/>
    /// 自機を画面内に制限させる
    /// </summary>
    /// <param name="_moveInput">移動方向</param>
    void Move(object _moveInput)
    {
        Vector2 moveInput = (Vector2)_moveInput; 
  
        //移動コルーチンがnullではない場合停止させる
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);

        }

        if(optionMoveCoroutine == null)
        {
            optionMoveCoroutine = StartCoroutine(nameof(OptionMoveCoroutine));
        }
        //移動方向は入力を正規化して返した数値、正規化によって斜めが早くなることを防ぎます
        moveDirection = moveInput.normalized;
        moveCoroutine = StartCoroutine(MoveCoroutine(accelerationTime, moveDirection * moveSpeed, Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right)));
        
        //移動させるであれば減速コルーチンを止める
        StopCoroutine(nameof(DecelerationCoroutine));
        //画面制限を始まる
        StartCoroutine(nameof(MoveRangeLimatationCoroutine));
  
    }

    /// <summary>
    /// 移動停止処理
    /// </summary>
    void StopMove()
    {
        //移動コルーチンがnullではない場合停止させる
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveDirection = Vector2.zero;
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
            //
            //物理演算させるからFixedUpdate使って精度を上げる
            yield return waitForFixedUpdate;
        }
    }

    /// <summary>
    /// 移動範囲を画面内に制限させる
    /// </summary>
    IEnumerator MoveRangeLimatationCoroutine()
    {
        while (true)
        {
            transform.position = Viewport.Instance.PlayerMoveablePosition(transform.position, paddingX, paddingY);
            yield return null;
        }
    }

    /// <summary>
    /// 減速終了時に自機を画面内に制限するコルーチンを停止して<br/>
    /// 無駄のループ処理を減らす
    /// </summary>
    /// <returns></returns>
    IEnumerator DecelerationCoroutine()
    {
        yield return waitDecelerationTime;

        StopCoroutine(MoveRangeLimatationCoroutine());
        StopCoroutine(OptionMoveCoroutine());
    }

    IEnumerator OptionMoveCoroutine()
    {
        while(true)
        {
            UpdateOptionPositions(transform.position);
            yield return null;
        }
    }

    void UpdateOptionPositions(Vector3 newPosition)
    {
        foreach (var option in optionManager.options)
        {
            option.UpdatePosition(newPosition);
        }
    }

    void OverDriveOn()
    {
        moveSpeed *= overdriveSpeedFactor;
    }

    void OverDriveOff()
    {
        moveSpeed /= overdriveSpeedFactor;
    }
}
