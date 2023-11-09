using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInput : 
    ScriptableObject, 
    InputActions.IGameplayActions,
    InputActions.IPauseMenuActions,
    InputActions.IGameOverScreenActions
{
    // 移動アクションのためにイベントを作成して
    public event UnityAction<Vector2> onMove = delegate { };

    public event UnityAction onStopMove = delegate { };

    //発射アクションのためのイベントです
    public event UnityAction onFire = delegate { };

    public event UnityAction onStopFire = delegate { };

    //回避するのためのイベントです
    public event UnityAction onDodge = delegate { };

    //爆発のためのイベントです
    public event UnityAction onOverdrive = delegate { };

    //一時停止のためのイベントです
    public event UnityAction onPause = delegate { };

    //開始のためのイベントです
    public event UnityAction onUnpause = delegate { };

    public event UnityAction onLaunchMissile = delegate { };

    public event UnityAction onConfirmGameOver = delegate { };

    // 新しいInput Systemのアクションへの参照。
    InputActions inputActions;

    // 有効にされた時に呼ばれるメソッド。
    void OnEnable()
    {
        inputActions = new InputActions();

        inputActions.Gameplay.SetCallbacks(this);
        inputActions.PauseMenu.SetCallbacks(this);
        inputActions.GameOverScreen.SetCallbacks(this);

    }

    // 無効にされた時に呼ばれるメソッド。
    void OnDisable()
    {
        DisableAllInputs();
    }

    /// <summary>
    /// 有効actionmapを変わり
    /// </summary>
    /// <param name="actionMap">変えたいactionMap</param>
    /// <param name="isUIInput">UIの選択か</param>
    void SwitchActionMap(InputActionMap actionMap, bool isUIInput)
    {
        inputActions.Disable();
        actionMap.Enable();

        if(isUIInput)
        {
            Cursor.visible = true;                     // マウスカーソルを可視にします。
            Cursor.lockState = CursorLockMode.None;    // マウスカーソルをロックしない。
        }
        else
        {
            Cursor.visible = false;                     // マウスカーソルを不可視にします。
            Cursor.lockState = CursorLockMode.Locked;   // マウスカーソルをロックする。
        }
    }

    /*
        Time.timeScaleが0になるときInputSystemの
        UpdateModeが In Fixed Updateだから入力が無効になる
        それを防ぐため、一時停止の時をUpdateModeをDynamicUpdate変える
     */
    /// <summary>
    /// 入力をProcessEventsInDynamicUpdateに変える
    /// </summary>
    public void SwitchToDynamicUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;

    /// <summary>
    /// 入力をProcessEventsInFixedUpdateに変える
    /// </summary>
    public void SwitchToFixedUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;

    /// <summary>
    /// 入力を無効化する
    /// </summary>
    public void DisableAllInputs()　=> inputActions.Disable();

    /// <summary>
    /// ゲーム内でキャラクターを操作する時に入力を有効化するメソッド。
    /// </summary>
    public void EnableGameplayInput() => SwitchActionMap(inputActions.Gameplay, false);

    /// <summary>
    /// 一時停止画面内の入力を有効化するメソッド
    /// </summary>
    public void EnablePauseMenuiInput() => SwitchActionMap(inputActions.PauseMenu, true);

    /// <summary>
    /// ゲームオーバー時の入力
    /// </summary>
    public void EnableGameOverScreenInput() => SwitchActionMap(inputActions.GameOverScreen, true);

    // 移動アクションがトリガーされた時に呼ばれるメソッド。
    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            //イベントの実行
            onMove.Invoke(context.ReadValue<Vector2>());
        }

        if(context.canceled)
        {
            onStopMove.Invoke();
        }
    }

    // 発射アクションがトリガーされた時に呼ばれるメソッド。
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onFire.Invoke();
        }

        if (context.canceled)
        {
            onStopFire.Invoke();
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
       if(context.performed)
        {
            onDodge.Invoke();
        }
    }

    public void OnOverdrive(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            onOverdrive.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            onPause.Invoke();
        }
    }

    public void OnUnpause(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            onUnpause.Invoke();
        }
    }

    public void OnLaunchMissile(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            onLaunchMissile.Invoke();
        }
    }

    public void OnConfirmGameOver(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            onConfirmGameOver.Invoke();
        }
    }
}
