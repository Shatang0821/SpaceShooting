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

    //HP�����񕜎���
    WaitForSeconds waitHealthRegenerateTime;

    private Vector2 lastMoveDirection;

    //HealthRegenerateCoroutine�𒆎~���邽�߂̓��ꕨ
    Coroutine healthRegenerateCoroutine;

    void Awake()
    {
        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        EventCenter.Subscribe(EventNames.Move, OnPlayerMove);
        EventCenter.Subscribe(EventNames.StopMove, OnPlayerStopMove);
    }

    void OnDisable()
    {
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
    }

    void OnPlayerStopMove()
    {
        lastMoveDirection = Vector2.zero;
    }


}
