using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{


    [Header("---- MOVE ----")]
    [Tooltip("����̓L�����N�^�[�̍ő呬�x�ł��B")]
    [SerializeField] float moveSpeed = 10f;

    [Tooltip("����̓L�����N�^�[�̉������Ԃł��B")]
    [SerializeField] float accelerationTime = 3f;   //��������

    [Tooltip("����̓L�����N�^�[�̌������Ԃł��B")]
    [SerializeField] float decelerationTime = 3f;   //��������

    [Tooltip("����̓L�����N�^�[�̏㉺�ړ��p�x�ł��B")]
    [SerializeField] float moveRotationAngle = 50f;

    //�v���C���[���
    private Player _player;
    private PlayerInput _input;
    private new Rigidbody2D rigidbody;

    //���f����XY�̔����T�C�Y
    private float paddingX;
    private float paddingY;

    //���͂����ړ�������ێ�����ϐ�
    Vector2 moveDirection;

    float t;                        //used for MoveCoroutine
    Vector2 previousVelocity;       //used for MoveCoroutine
    Quaternion previousRotation;    //used for MoveCoroutine

    WaitForSeconds waitDecelerationTime;//��������

    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();   //used for MoveCoroutine

    Coroutine moveCoroutine;

    private void Awake()
    {
        _player = GetComponent<Player>();
        rigidbody = GetComponent<Rigidbody2D>();
        //�T�C�Y�擾
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x / 2f;
        paddingY = size.y / 2f;

        waitDecelerationTime = new WaitForSeconds(decelerationTime);
    }

    private void Start()
    {
        _input =_player.input;

        rigidbody.gravityScale = 0f;//�d�͂�0
    }

    private void OnEnable()
    {
        EventCenter.Subscribe("Move", Move);
        //_input.onMove += Move;
        EventCenter.Subscribe("stopMove", StopMove);
    }

    private void OnDisable()
    {
        EventCenter.Unsubscribe("Move", Move);
        EventCenter.Unsubscribe("stopMove", StopMove);
    }

    void Move(object _moveInput)
    {
        Vector2 moveInput = (Vector2)_moveInput; 
        // Vector2 moveAmount = moveInput * moveSpeed;
        // rigidbody.velocity = moveAmount;
        //�ړ��R���[�`����null�ł͂Ȃ��ꍇ��~������
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);

        }
        //�L�[��value�𐳋K��������1��-1�ɂ���
        moveDirection = moveInput.normalized;
        //Quaternion moveRotation = Quaternion.AngleAxis(moveRotationAngle * moveInput.y,Vector3.right);
        moveCoroutine = StartCoroutine(MoveCoroutine(accelerationTime, moveDirection * moveSpeed, Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right)));
        //�ړ�������ł���Ό����R���[�`�����~�߂�
        StopCoroutine(nameof(DecelerationCoroutine));
        //��ʐ������n�܂�
        StartCoroutine(nameof(MoveRangeLimatationCoroutine));
    }

    void StopMove()
    {
        //rigidbody.velocity = Vector2.zero;
        //�ړ��R���[�`����null�ł͂Ȃ��ꍇ��~������
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveDirection = Vector2.zero;
        //���x�Ɖ�]���������Ԃɂ���ď��X�ɏ����ɖ߂�
        moveCoroutine = StartCoroutine(MoveCoroutine(decelerationTime, Vector2.zero, Quaternion.identity));
        //�������Ԃ��I���Ɖ�ʐ������~�߂�,,�ړ����Ă��Ȃ����߉�ʊO�ɏo�邱�ƂȂ�
        StartCoroutine(nameof(DecelerationCoroutine));
    }

    /// <summary>
    ///  ���x�ύX������
    /// </summary>
    /// <param name="time">�ύX����</param>
    /// <param name="moveVelocity">�ړ����x</param>
    /// <param name="moveRotation">��]���</param>
    /// <returns></returns>
    IEnumerator MoveCoroutine(float time, Vector2 moveVelocity, Quaternion moveRotation)
    {
        t = 0f;
        previousVelocity = rigidbody.velocity;
        previousRotation = transform.rotation;

        while (t < 1f)
        {
            t += Time.fixedDeltaTime / time;
            //t��0����1�̊Ԃ�velocity��␳������t��1�ɂȂ�ƍő呬�x�ɒB���܂�
            rigidbody.velocity = Vector2.Lerp(previousVelocity, moveVelocity, t);
            //��Ɠ�������
            transform.rotation = Quaternion.Lerp(previousRotation, moveRotation, t);

            //�������Z�����邩��FixedUpdate�g���Đ��x���グ��
            yield return waitForFixedUpdate;
        }
    }

    /// <summary>
    /// �ړ��͈͂���ʓ��ɐ���������
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
