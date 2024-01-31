using Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("---- MOVE ----")]
    [Tooltip("�L�����N�^�[�̍ő呬�x�ł��B")]
    [SerializeField] float moveSpeed = 10f;

    [Tooltip("�L�����N�^�[�̓����ԃX�s�[�h�{��")]
    [SerializeField] float overdriveSpeedFactor = 1.2f;//�X�s�[�h��1.2�{

    [Tooltip("�L�����N�^�[�̉������Ԃł��B")]
    [SerializeField] float accelerationTime = 3f;   //��������

    [Tooltip("�L�����N�^�[�̌������Ԃł��B")]
    [SerializeField] float decelerationTime = 3f;   //��������

    [Tooltip("�L�����N�^�[�̏㉺��]�p�x�ł��B")]
    [SerializeField] float moveRotationAngle = 50f;

    //�v���C���[���
    private Player _player;

    private PlayerInput _input;

    private new Rigidbody2D rigidbody;

    private OptionManager optionManager;

    //���f����XY�̔����T�C�Y
    private float paddingX;
    private float paddingY;

    //���͂����ړ�������ێ�����ϐ�
    Vector2 moveDirection;
    /*
        ���O�ɒ�`���Ă����ɂ���ČJ��Ԃ���`��h��
     */
    float t;                        //used for MoveCoroutine
    Vector2 previousVelocity;       //used for MoveCoroutine
    Quaternion previousRotation;    //used for MoveCoroutine

    //�R���[�`���֘A�ϐ�
    WaitForSeconds waitDecelerationTime;//��������

    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();//FixedUpdate�������Z�œK

    Coroutine moveCoroutine;//�ړ��R���[�`���̏d���J�n��h�����߂̓������

    Coroutine optionMoveCoroutine;

    /// <summary>
    /// ������
    /// </summary>
    void Initialized()
    {
        _player = GetComponent<Player>();
        rigidbody = GetComponent<Rigidbody2D>();
        optionManager = GetComponentInChildren<OptionManager>();
        waitDecelerationTime = new WaitForSeconds(decelerationTime);
    }

    /// <summary>
    /// �@�̃T�C�Y�擾
    /// </summary>
    void SetSize()
    {
        //�T�C�Y�擾
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

        rigidbody.gravityScale = 0f;//�d�͂�0
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
    /// �ړ�����<br/>
    /// �R���[�`�����n�܂鏈��<br/>
    /// ���@����ʓ��ɐ���������
    /// </summary>
    /// <param name="_moveInput">�ړ�����</param>
    void Move(object _moveInput)
    {
        Vector2 moveInput = (Vector2)_moveInput; 
  
        //�ړ��R���[�`����null�ł͂Ȃ��ꍇ��~������
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);

        }

        if(optionMoveCoroutine == null)
        {
            optionMoveCoroutine = StartCoroutine(nameof(OptionMoveCoroutine));
        }
        //�ړ������͓��͂𐳋K�����ĕԂ������l�A���K���ɂ���Ď΂߂������Ȃ邱�Ƃ�h���܂�
        moveDirection = moveInput.normalized;
        moveCoroutine = StartCoroutine(MoveCoroutine(accelerationTime, moveDirection * moveSpeed, Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right)));
        
        //�ړ�������ł���Ό����R���[�`�����~�߂�
        StopCoroutine(nameof(DecelerationCoroutine));
        //��ʐ������n�܂�
        StartCoroutine(nameof(MoveRangeLimatationCoroutine));
  
    }

    /// <summary>
    /// �ړ���~����
    /// </summary>
    void StopMove()
    {
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
            //
            //�������Z�����邩��FixedUpdate�g���Đ��x���グ��
            yield return waitForFixedUpdate;
        }
    }

    /// <summary>
    /// �ړ��͈͂���ʓ��ɐ���������
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
    /// �����I�����Ɏ��@����ʓ��ɐ�������R���[�`�����~����<br/>
    /// ���ʂ̃��[�v���������炷
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
