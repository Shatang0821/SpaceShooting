using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(Rigidbody2D))]
public class Player : Character
{
    [SerializeField] StatsBar_HUD statsBar_HUD;

    //�񕜃X�C�b�`
    [SerializeField] bool regenerateHealth = true;

    //�񕜂܂ł̎���
    [SerializeField] float healthRegenerateTime;

    //�񕜃p�[�Z���g
    [SerializeField, Range(0f, 1f)] float healthRegeneratePercent;

    [Header("---- INPUT ----")]
    public PlayerInput input;

    #region PlayerDodge
    [Header("----- DODGE -----")]

    [SerializeField] AudioData dodgeSFX;//���ʉ�

    [Tooltip("����̓L�����N�^�[�̃G�l���M�[���՗ʂł��B")]
    [SerializeField,Range(0,100)] int dodgeEnergyCost = 25;

    [SerializeField] float maxRoll = 720f;//�ő��]�p�x

    [SerializeField] float rollSpeed = 360f;//��]���x
    [SerializeField] Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);//�X�P�[���ω�

    bool isDodging = false; //�_�b�W�@�]�����킷

    float currentRoll;//���݉�]�l

    float dodgeDuration;//�_�b�W��������

    readonly float slowMotionDuration = 1f;//�o���b�g�^�C��slowout����
    #endregion

    #region Overdrive�@
    //���E�˔j
    [HideInInspector] public bool isOverdriving = false;

    [SerializeField] int overdriveDodgeFactor = 2;  //�_�b�W���Ղ��Q�{�𑝂₷

    //[SerializeField] float overdriveSpeedFactor = 1.2f;//�X�s�[�h��1.2�{

    [SerializeField] float overdriveFireFactor = 1.2f;//�U���Ԋu1.2�{�k��
    #endregion

    [HideInInspector] public WaitForSeconds waitForOverdriveFireInterval;//�I�[�o�[�h���C�u�̍U���Ԋu

    //HP�����񕜎���
    WaitForSeconds waitHealthRegenerateTime;

    new Collider2D collider;

    private Vector2 lastMoveDirection;

    //HealthRegenerateCoroutine�𒆎~���邽�߂̓��ꕨ
    Coroutine healthRegenerateCoroutine;

    void Awake()
    {

        collider = GetComponent<Collider2D>();
        


        //waitForFireInterval = new WaitForSeconds(fireInterval);

        //waitForOverdriveFireInterval = new WaitForSeconds(fireInterval /= overdriveFireFactor);

        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);

        dodgeDuration = maxRoll / rollSpeed;//�ő��]�p�x/��]���x = ����
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        //�C�x���g�̃T�u�X�N���C�u
        input.onDodge += Dodge;
        input.onOverdrive += Overdrive;

        PlayerOverdrive.on += OverdriveOn;
        PlayerOverdrive.off += OverdriveOff;

        EventCenter.Subscribe(EventNames.Move, OnPlayerMove);
        EventCenter.Subscribe(EventNames.StopMove, OnPlayerStopMove);
    }

    void OnDisable()
    {
        //�C�x���g�̃A���T�u�X�N���C�u

        input.onDodge -= Dodge;
        input.onOverdrive -= Overdrive;

        PlayerOverdrive.on -= OverdriveOn;
        PlayerOverdrive.off -= OverdriveOff;

        EventCenter.Unsubscribe(EventNames.Move, OnPlayerMove);
        EventCenter.Unsubscribe(EventNames.StopMove, OnPlayerStopMove);
    }
    // Start is called before the first frame update
    void Start()
    {
        statsBar_HUD.Initialize(health, maxHealth);

        input.EnableGameplayInput();
    }

    /// <summary>
    /// �_���[�W���󂯂鏈��
    /// </summary>
    /// <param name="damage">�_���[�W��</param>
    public override void TakenDamage(float damage)
    {
        base.TakenDamage(damage);
        //�o�[���X�V
        statsBar_HUD.UpdateStats(health, maxHealth);

        //�A�N�e�B�u��ԂȂ��Ă��鎞
        if (gameObject.activeSelf)
        {
            //�e�ƂԂ���Ǝ~�܂�Ƃ�������A�����h�����߂ɓ��͂�����Ȃ瓮������
            EventCenter.TriggerEvent(EventNames.Move, lastMoveDirection);
            //

            //�����񕜒��ł����
            if (regenerateHealth)
            {
                //�����R���[�`�����n�܂������ł��_���[�W���󂯂�ƃ��Z�b�g�����܂�
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
        //�o�[���X�V����
        statsBar_HUD.UpdateStats(health, maxHealth);
    }

    public override void Die()
    {
        //GameManager.onGameOver��null�łȂ��ꍇ�̂�Invoke���Ăяo��
        //?��null�������Z�q
        GameManager.onGameOver?.Invoke();
        GameManager.GameState = GameState.GameOver;
        statsBar_HUD.UpdateStats(0f, maxHealth);
        base.Die();
    }

    void OnPlayerMove(object moveInput)
    {
        lastMoveDirection = (Vector2)moveInput;
        Debug.Log(lastMoveDirection);
    }

    void OnPlayerStopMove()
    {
        lastMoveDirection = Vector2.zero;
        Debug.Log(lastMoveDirection);
    }

    #region DODGE
    void Dodge()
    {
        //�����_�b�W���ł���Ώ��������Ȃ��܂��A�G�l���M�[������Ȃ��Ƃ������l
        if (isDodging || !PlayerEnergy.Instance.IsEnough(dodgeEnergyCost)) return;
        //�����łȂ���Ώ����ɓ���
        StartCoroutine(nameof(DodgeCoroutine));
        // Change Player's scale
    }
    /// <summary>
    /// �_�b�W���s�R���[�`��
    /// </summary>
    /// <returns></returns>
    IEnumerator DodgeCoroutine()
    {
        isDodging = true;
        AudioManager.Instance.PlayRandomSFX(dodgeSFX);
        // �G�l���M�[������
        PlayerEnergy.Instance.Use(dodgeEnergyCost);

        //���̂ƒʉ߂ł���悤�ɂ���
        collider.isTrigger = true;

        // ������]��0�ɂ���
        currentRoll = 0f;

        //�o���b�g�^�C�����J�n
        TimeController.Instance.BulletTime(slowMotionDuration, slowMotionDuration);

        //���݉�]���ő��]��菬�����ꍇ���[�v������
        while (currentRoll < maxRoll)
        {
            currentRoll += rollSpeed * Time.deltaTime;
            //��]������
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
            //�x�W�F�Ȑ����g���ē������Ȃ߂炩�ɂ���
            //scale��1����0.5�܂�1�ɖ߂�悤�ɂ���
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
        //�G�l���M�[������Ȃ��ꍇ���������Ȃ�
        if (!PlayerEnergy.Instance.IsEnough(PlayerEnergy.MAX)) return;

        PlayerOverdrive.on.Invoke();
    }

    void OverdriveOn()
    {
        
        isOverdriving = true;
        dodgeEnergyCost *= overdriveDodgeFactor;
        //moveSpeed *= overdriveSpeedFactor;
    }

    void OverdriveOff()
    {
        isOverdriving = false;
        dodgeEnergyCost /= overdriveDodgeFactor;
        //moveSpeed /= overdriveSpeedFactor;
    }
    #endregion


}
