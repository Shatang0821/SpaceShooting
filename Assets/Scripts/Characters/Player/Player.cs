using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Player : Character
{
    [SerializeField] StatsBar_HUD statsBar_HUD;

    //�񕜃X�C�b�`
    [SerializeField] bool regenerateHealth = true;

    //�񕜂܂ł̎���
    [SerializeField] float healthRegenerateTime;

    //�񕜃p�[�Z���g
    [SerializeField,Range(0f, 1f)] float healthRegeneratePercent;

    #region PlayerAttributes

    [Header("---- INPUT ----")]
    [SerializeField] PlayerInput input;

    [Header("---- PlayerAttributes ----")]
    
    [Tooltip("����̓L�����N�^�[�̍ő呬�x�ł��B")]
    [SerializeField] float moveSpeed = 10f;

    [Tooltip("����̓L�����N�^�[�̉������Ԃł��B")]
    [SerializeField] float accelerationTime = 3f;   //��������

    [Tooltip("����̓L�����N�^�[�̌������Ԃł��B")]
    [SerializeField] float decelerationTime = 3f;   //��������

    [Tooltip("����̓L�����N�^�[�̏㉺�ړ��p�x�ł��B")]
    [SerializeField] float moveRotationAngle = 50f;

    [Tooltip("����̓L�����N�^�[�̂��[�̐��l�ł��B")]
    [SerializeField] float paddingx = 0.2f;

    [Tooltip("����̓L�����N�^�[�̂��[�̐��l�ł��B")]
    [SerializeField] float paddingy = 0.2f;
    #endregion

    #region ProjectileAttributes

    [Header("---- ProjectileAttributes ----")]
    [Tooltip("����̓L�����N�^�[�̒e�I�u�W�F�N�g�ł��B")]
    [SerializeField] GameObject projectile1; //�e�I�u�W�F�N�g
    [SerializeField] GameObject projectile2; //�e�I�u�W�F�N�g
    [SerializeField] GameObject projectile3; //�e�I�u�W�F�N�g

    [Tooltip("����̓L�����N�^�[�̒e���ˈʒu�ł��B")]
    [SerializeField] Transform muzzleMiddle;      //�e���ˈʒu
    [SerializeField] Transform muzzleTop;      //�e���ˈʒu
    [SerializeField] Transform muzzleBottom;      //�e���ˈʒu

    [Tooltip("����̓L�����N�^�[�̃p���[�ł��B")]
    [SerializeField, Range(0, 2)] int weaponPower = 0;

    [Tooltip("����̓L�����N�^�[�̒e���ˊԊu�ł��B")]
    [SerializeField] float fireInterval = 0.2f;    //�e���ˊԊu
    
    #endregion

    WaitForSeconds waitForFireInterval;

    //HP�����񕜎���
    WaitForSeconds waitHealthRegenerateTime;

    new Rigidbody2D rigidbody;

    Coroutine moveCoroutine;

    //HealthRegenerateCoroutine�𒆎~���邽�߂̓��ꕨ
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
        //�������ꂩ�炱�̔��ˊԊu���Q�[�����ɕҏW����Ȃ�֐�������ĊԊu��ς���Ƃ���
        //�ȉ��̕����֐����ɒu�����Ƃɂ��A����ҏW����Ƃ����V�������܂��B
        waitForFireInterval = new WaitForSeconds(fireInterval);
        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);

        statsBar_HUD.Initialize(health, maxHealth);

        rigidbody.gravityScale = 0f;//�d�͂�0

        input.EnableGameplayInput();

    }


    public override void TakenDamage(float damage)
    {
        base.TakenDamage(damage);
        statsBar_HUD.UpdateStats(health, maxHealth);

        //�A�N�e�B�u��ԂȂ��Ă��鎞
        if(gameObject.activeSelf)
        {
            if (regenerateHealth)
            {
                //�����R���[�`�����n�܂������ł��_���[�W���󂯂�ƃ��Z�b�g�����܂�
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
        //StopCoroutine(FireCoroutine());//��p���Ȃ�
        StopCoroutine(nameof(FireCoroutine));
    }

    IEnumerator FireCoroutine()
    {
        while (true)
        {
            switch (weaponPower)
            {
                case 0:
                    PoolManager.Release(projectile1, muzzleMiddle.position);//�e�𐶐�����
                    break;
                case 1:
                    PoolManager.Release(projectile1, muzzleTop.position);//�e�𐶐�����
                    PoolManager.Release(projectile1, muzzleBottom.position);//�e�𐶐�����
                    break;
                case 2:
                    PoolManager.Release(projectile1, muzzleMiddle.position);//�e�𐶐�����
                    PoolManager.Release(projectile2, muzzleTop.position);//�e�𐶐�����
                    PoolManager.Release(projectile3, muzzleBottom.position);//�e�𐶐�����
                    break;
                default:
                    break;

            }

            yield return waitForFireInterval;
        }
    }
    #endregion
}
