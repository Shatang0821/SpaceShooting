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

    public void DisableAllInputs()//���͂𖳌�������
    {
        inputActions.Gameplay.Disable();
    }

    public void EnableGameplayInput()//�Q�[�����ŃL�������삷�鎞
    {
        inputActions.Gameplay.Enable();//���͂�L��������

        Cursor.visible = false;//�}�E�X�s��
        Cursor.lockState = CursorLockMode.Locked;//�}�E�X�s�ړ�
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
