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
    [SerializeField, Range(0f, 1f)] float healthRegeneratePercent;

    [Header("---- INPUT ----")]
    public PlayerInput input;

    #region ProjectileAttributes

    [Header("---- FIRE ----")]
    [Tooltip("����̓L�����N�^�[�̒e�I�u�W�F�N�g�ł��B")]
    [SerializeField] GameObject projectile1; //�e�I�u�W�F�N�g
    [SerializeField] GameObject projectile2; //�e�I�u�W�F�N�g
    [SerializeField] GameObject projectile3; //�e�I�u�W�F�N�g
    [SerializeField] GameObject projectileOverdrive;

    [Tooltip("����̓L�����N�^�[�̒e���ˈʒu�ł��B")]
    [SerializeField] Transform muzzleMiddle;      //�e���ˈʒu
    [SerializeField] Transform muzzleTop;      //�e���ˈʒu
    [SerializeField] Transform muzzleBottom;      //�e���ˈʒu

    [SerializeField] AudioData projectileLaunchSFX;   //���ˌ��ʉ�

    [Tooltip("����̓L�����N�^�[�̃p���[�ł��B")]
    [SerializeField, Range(0, 2)] int weaponPower = 0;

    [Tooltip("����̓L�����N�^�[�̒e���ˊԊu�ł��B")]
    [SerializeField] float fireInterval = 0.2f;    //�e���ˊԊu



    #endregion

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
    bool isOverdriving = false;

    [SerializeField] int overdriveDodgeFactor = 2;  //�_�b�W���Ղ��Q�{�𑝂₷

    //[SerializeField] float overdriveSpeedFactor = 1.2f;//�X�s�[�h��1.2�{

    [SerializeField] float overdriveFireFactor = 1.2f;//�U���Ԋu1.2�{�k��
    #endregion



    WaitForSeconds waitForFireInterval;//�U���Ԋu

    WaitForSeconds waitForOverdriveFireInterval;//�I�[�o�[�h���C�u�̍U���Ԋu

    //HP�����񕜎���
    WaitForSeconds waitHealthRegenerateTime;


    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();   //used for MoveCoroutine


    new Collider2D collider;

    MissileSystem missile;

//    Coroutine moveCoroutine;

    //HealthRegenerateCoroutine�𒆎~���邽�߂̓��ꕨ
    Coroutine healthRegenerateCoroutine;

    void Awake()
    {

        collider = GetComponent<Collider2D>();
        missile = GetComponent<MissileSystem>();


        waitForFireInterval = new WaitForSeconds(fireInterval);
        waitForOverdriveFireInterval = new WaitForSeconds(fireInterval /= overdriveFireFactor);
        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);

        dodgeDuration = maxRoll / rollSpeed;//�ő��]�p�x/��]���x = ����
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        //�C�x���g�̃T�u�X�N���C�u

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
        //�C�x���g�̃A���T�u�X�N���C�u

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
            //Move(moveDirection);
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

    #region FIRE

    //�C�x���g�����s�����炱����Ăяo��
    void Fire()
    {
        StartCoroutine(nameof(FireCoroutine));
    }
    //�L�[��b������~�܂�
    void StopFire()
    {
        //StopCoroutine(FireCoroutine());//��p���Ȃ�
        StopCoroutine(nameof(FireCoroutine));
    }

    /// <summary>
    //   �U���R���[�`��
    /// </summary>
    /// <returns></returns>
    IEnumerator FireCoroutine()
    {
        while (true)
        {
            switch (weaponPower)
            {
                case 0:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleMiddle.position);//�e�𐶐�����
                    break;
                case 1:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleTop.position);//�e�𐶐�����
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleBottom.position);//�e�𐶐�����
                    break;
                case 2:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleMiddle.position);//�e�𐶐�����
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile2, muzzleTop.position);//�e�𐶐�����
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile3, muzzleBottom.position);//�e�𐶐�����
                    break;
                default:
                    break;

            }

            AudioManager.Instance.PlayRandomSFX(projectileLaunchSFX);
            //�I�[�o�[�h���C�u��ԂȂ炻�̍U�����x
            yield return isOverdriving ? waitForOverdriveFireInterval : waitForFireInterval;
            
        }
    }
    #endregion

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

    void LaunchMissile()
    {
        missile.Launch(muzzleMiddle);
    }
}
