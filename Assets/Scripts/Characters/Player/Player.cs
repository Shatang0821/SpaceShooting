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


    #region Overdrive�@
    //���E�˔j
    [HideInInspector] public bool isOverdriving = false;

    [SerializeField] int overdriveDodgeFactor = 2;  //�_�b�W���Ղ��Q�{�𑝂₷

    //[SerializeField] float overdriveSpeedFactor = 1.2f;//�X�s�[�h��1.2�{

    //[SerializeField] float overdriveFireFactor = 1.2f;//�U���Ԋu1.2�{�k��
    #endregion

    [HideInInspector] public WaitForSeconds waitForOverdriveFireInterval;//�I�[�o�[�h���C�u�̍U���Ԋu

    //HP�����񕜎���
    WaitForSeconds waitHealthRegenerateTime;



    private Vector2 lastMoveDirection;

    //HealthRegenerateCoroutine�𒆎~���邽�߂̓��ꕨ
    Coroutine healthRegenerateCoroutine;

    void Awake()
    {
       
        //waitForFireInterval = new WaitForSeconds(fireInterval);

        //waitForOverdriveFireInterval = new WaitForSeconds(fireInterval /= overdriveFireFactor);

        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);


    }

    protected override void OnEnable()
    {
        base.OnEnable();

        //�C�x���g�̃T�u�X�N���C�u

        input.onOverdrive += Overdrive;

        PlayerOverdrive.on += OverdriveOn;
        PlayerOverdrive.off += OverdriveOff;

        EventCenter.Subscribe(EventNames.Move, OnPlayerMove);
        EventCenter.Subscribe(EventNames.StopMove, OnPlayerStopMove);
    }

    void OnDisable()
    {
        //�C�x���g�̃A���T�u�X�N���C�u

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
        //dodgeEnergyCost *= overdriveDodgeFactor;
        //moveSpeed *= overdriveSpeedFactor;
    }

    void OverdriveOff()
    {
        isOverdriving = false;
        //dodgeEnergyCost /= overdriveDodgeFactor;
        //moveSpeed /= overdriveSpeedFactor;
    }
    #endregion


}
