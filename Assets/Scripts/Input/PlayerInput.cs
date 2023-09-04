using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInput : ScriptableObject, InputActions.IGameplayActions
{
    // 移動アクションのためのイベントです。
    public event UnityAction<Vector2> onMove = delegate { };

    public event UnityAction onStopMove = delegate { };

    //発射アクションのためのイベントです
    public event UnityAction onFire = delegate { };

    public event UnityAction onStopFire = delegate { };

    // 新しいInput Systemのアクションへの参照。
    InputActions inputActions;

    // 有効にされた時に呼ばれるメソッド。
    void OnEnable()
    {
        inputActions = new InputActions();

        inputActions.Gameplay.SetCallbacks(this);
    }

    // 無効にされた時に呼ばれるメソッド。
    void OnDisable()
    {
        DisableAllInputs();
    }

    public void DisableAllInputs()//入力を無効化する
    {
        inputActions.Gameplay.Disable();
    }

    // ゲーム内でキャラクターを操作する時に入力を有効化するメソッド。
    public void EnableGameplayInput()
    {
        inputActions.Gameplay.Enable();             //入力を有効化する

        Cursor.visible = false;                     // マウスカーソルを不可視にします。
        Cursor.lockState = CursorLockMode.Locked;   // マウスカーソルを不可視にします。
    }

    // 移動アクションがトリガーされた時に呼ばれるメソッド。
    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
                onMove.Invoke(context.ReadValue<Vector2>());
        }

        if(context.phase == InputActionPhase.Canceled)
        {
            onStopMove.Invoke();
        }
    }

    // 発射アクションがトリガーされた時に呼ばれるメソッド。まだ実装されていません。
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            onFire.Invoke();
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            onStopFire.Invoke();
        }
    }
}
