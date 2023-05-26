using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInput : ScriptableObject, InputActions.IGameplayActions
{
    public event UnityAction<Vector2> onMove = delegate { };
    public event UnityAction onStopMove = delegate { };
    InputActions inputActions;

    void OnEnable()
    {
        inputActions = new InputActions();

        inputActions.Gameplay.SetCallbacks(this);
    }

    void OnDisable()
    {
        DisableAllInputs();
    }

    public void DisableAllInputs()//入力を無効化する
    {
        inputActions.Gameplay.Disable();
    }

    public void EnableGameplayInput()//ゲーム内でキャラ操作する時
    {
        inputActions.Gameplay.Enable();//入力を有効化する

        Cursor.visible = false;//マウス不可見
        Cursor.lockState = CursorLockMode.Locked;//マウス不移動
    }

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
}
