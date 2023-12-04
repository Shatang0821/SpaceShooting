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

    //回復スイッチ
    [SerializeField] bool regenerateHealth = true;

    //回復までの時間
    [SerializeField] float healthRegenerateTime;

    //回復パーセント
    [SerializeField, Range(0f, 1f)] float healthRegeneratePercent;

    [Header("---- INPUT ----")]
    public PlayerInput input;


    #region Overdrive　
    //限界突破
    [HideInInspector] public bool isOverdriving = false;

    [SerializeField] int overdriveDodgeFactor = 2;  //ダッジ消耗を２倍を増やす

    //[SerializeField] float overdriveSpeedFactor = 1.2f;//スピードを1.2倍

    //[SerializeField] float overdriveFireFactor = 1.2f;//攻撃間隔1.2倍縮む
    #endregion

    [HideInInspector] public WaitForSeconds waitForOverdriveFireInterval;//オーバードライブの攻撃間隔

    //HP自動回復時間
    WaitForSeconds waitHealthRegenerateTime;



    private Vector2 lastMoveDirection;

    //HealthRegenerateCoroutineを中止するための入れ物
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

        //イベントのサブスクライブ

        input.onOverdrive += Overdrive;

        PlayerOverdrive.on += OverdriveOn;
        PlayerOverdrive.off += OverdriveOff;

        EventCenter.Subscribe(EventNames.Move, OnPlayerMove);
        EventCenter.Subscribe(EventNames.StopMove, OnPlayerStopMove);
    }

    void OnDisable()
    {
        //イベントのアンサブスクライブ

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
            EventCenter.TriggerEvent(EventNames.Move, lastMoveDirection);
            //

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
        //エネルギーが足りない場合処理させない
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
