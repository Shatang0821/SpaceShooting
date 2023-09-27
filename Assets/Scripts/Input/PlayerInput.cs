using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInput : ScriptableObject, InputActions.IGameplayActions
{
    // �ړ��A�N�V�����̂��߂̃C�x���g�ł��B
    public event UnityAction<Vector2> onMove = delegate { };

    public event UnityAction onStopMove = delegate { };

    //���˃A�N�V�����̂��߂̃C�x���g�ł�
    public event UnityAction onFire = delegate { };

    public event UnityAction onStopFire = delegate { };

    public event UnityAction onDodge = delegate { };

    // �V����Input System�̃A�N�V�����ւ̎Q�ƁB
    InputActions inputActions;

    // �L���ɂ��ꂽ���ɌĂ΂�郁�\�b�h�B
    void OnEnable()
    {
        inputActions = new InputActions();

        inputActions.Gameplay.SetCallbacks(this);
    }

    // �����ɂ��ꂽ���ɌĂ΂�郁�\�b�h�B
    void OnDisable()
    {
        DisableAllInputs();
    }

    public void DisableAllInputs()//���͂𖳌�������
    {
        inputActions.Gameplay.Disable();
    }

    // �Q�[�����ŃL�����N�^�[�𑀍삷�鎞�ɓ��͂�L�������郁�\�b�h�B
    public void EnableGameplayInput()
    {
        inputActions.Gameplay.Enable();             //���͂�L��������

        Cursor.visible = false;                     // �}�E�X�J�[�\����s���ɂ��܂��B
        Cursor.lockState = CursorLockMode.Locked;   // �}�E�X�J�[�\����s���ɂ��܂��B
    }

    // �ړ��A�N�V�������g���K�[���ꂽ���ɌĂ΂�郁�\�b�h�B
    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            onMove.Invoke(context.ReadValue<Vector2>());
        }

        if(context.canceled)
        {
            onStopMove.Invoke();
        }
    }

    // ���˃A�N�V�������g���K�[���ꂽ���ɌĂ΂�郁�\�b�h�B
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
}
