using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{


    [Header("---- MOVE ----")]
    [Tooltip("これはキャラクターの最大速度です。")]
    [SerializeField] float moveSpeed = 10f;

    [Tooltip("これはキャラクターの加速時間です。")]
    [SerializeField] float accelerationTime = 3f;   //加速時間

    [Tooltip("これはキャラクターの減速時間です。")]
    [SerializeField] float decelerationTime = 3f;   //減速時間

    [Tooltip("これはキャラクターの上下移動角度です。")]
    [SerializeField] float moveRotationAngle = 50f;

    //プレイヤー情報
    private Player _player;
    private PlayerInput _input;
    private new Rigidbody2D rigidbody;

    //モデルのXYの半分サイズ
    private float paddingX;
    private float paddingY;

    //入力した移動方向を保持する変数
    Vector2 moveDirection;

    float t;                        //used for MoveCoroutine
    Vector2 previousVelocity;       //used for MoveCoroutine
    Quaternion previousRotation;    //used for MoveCoroutine

    WaitForSeconds waitDecelerationTime;//減速時間

    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();   //used for MoveCoroutine

    Coroutine moveCoroutine;

    private void Awake()
    {
        _player = GetComponent<Player>();
        rigidbody = GetComponent<Rigidbody2D>();
        //サイズ取得
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x / 2f;
        paddingY = size.y / 2f;

        waitDecelerationTime = new WaitForSeconds(decelerationTime);
    }

    private void Start()
    {
        _input =_player.input;

        rigidbody.gravityScale = 0f;//重力を0
    }

    private void OnEnable()
    {
        EventCenter.Subscribe(EventNames.Move, Move);
        EventCenter.Subscribe(EventNames.StopMove, StopMove);
    }

    private void OnDisable()
    {
        EventCenter.Unsubscribe(EventNames.Move, Move);
        EventCenter.Unsubscribe(EventNames.StopMove, StopMove);
    }

    void Move(object _moveInput)
    {
        Vector2 moveInput = (Vector2)_moveInput; 
  
        //移動コルーチンがnullではない場合停止させる
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);

        }
        //移動方向は入力を正規化して返した数値、正規化によって斜めが早くなることを防ぎます
        moveDirection = moveInput.normalized;

        moveCoroutine = StartCoroutine(MoveCoroutine(accelerationTime, moveDirection * moveSpeed, Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right)));
        //移動させるであれば減速コルーチンを止める
        StopCoroutine(nameof(DecelerationCoroutine));
        //画面制限を始まる
        StartCoroutine(nameof(MoveRangeLimatationCoroutine));
    }

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
        while (true)
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
}
