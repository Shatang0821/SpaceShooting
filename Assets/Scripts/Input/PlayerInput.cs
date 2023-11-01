using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInput : ScriptableObject, InputActions.IGameplayActions,InputActions.IPauseMenuActions
{
    // �ړ��A�N�V�����̂��߂̃C�x���g�ł��B
    public event UnityAction<Vector2> onMove = delegate { };

    public event UnityAction onStopMove = delegate { };

    //���˃A�N�V�����̂��߂̃C�x���g�ł�
    public event UnityAction onFire = delegate { };

    public event UnityAction onStopFire = delegate { };

    //�������̂��߂̃C�x���g�ł�
    public event UnityAction onDodge = delegate { };

    //�����̂��߂̃C�x���g�ł�
    public event UnityAction onOverdrive = delegate { };

    //�ꎞ��~�̂��߂̃C�x���g�ł�
    public event UnityAction onPause = delegate { };

    //�J�n�̂��߂̃C�x���g�ł�
    public event UnityAction onUnpause = delegate { };

    public event UnityAction onLaunchMissile = delegate { };

    // �V����Input System�̃A�N�V�����ւ̎Q�ƁB
    InputActions inputActions;

    // �L���ɂ��ꂽ���ɌĂ΂�郁�\�b�h�B
    void OnEnable()
    {
        inputActions = new InputActions();

        inputActions.Gameplay.SetCallbacks(this);
        inputActions.PauseMenu.SetCallbacks(this);
    }

    // �����ɂ��ꂽ���ɌĂ΂�郁�\�b�h�B
    void OnDisable()
    {
        DisableAllInputs();
    }

    void SwitchActionMap(InputActionMap actionMap, bool isUIInput)
    {
        inputActions.Disable();
        actionMap.Enable();

        if(isUIInput)
        {
            Cursor.visible = true;                     // �}�E�X�J�[�\����s���ɂ��܂��B
            Cursor.lockState = CursorLockMode.None;   // �}�E�X�J�[�\����s���ɂ��܂��B
        }
        else
        {
            Cursor.visible = false;                     // �}�E�X�J�[�\����s���ɂ��܂��B
            Cursor.lockState = CursorLockMode.Locked;   // �}�E�X�J�[�\����s���ɂ��܂��B
        }
    }

    /*
        Time.timeScale��0�ɂȂ�Ƃ�InputSystem��
        UpdateMode�� In Fixed Update��������͂������ɂȂ�
        �����h�����߁A�ꎞ��~�̎���UpdateMode��ς���
     */
    public void SwitchToDynamicUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
    
    public void SwitchToFixedUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
    
    //���͂𖳌�������
    public void DisableAllInputs()�@=> inputActions.Disable();

    /// <summary>
    /// �Q�[�����ŃL�����N�^�[�𑀍삷�鎞�ɓ��͂�L�������郁�\�b�h�B
    /// </summary>
    public void EnableGameplayInput() => SwitchActionMap(inputActions.Gameplay, false);

    /// <summary>
    /// �ꎞ��~��ʓ��̓��͂�L�������郁�\�b�h
    /// </summary>
    public void EnablePauseMenuiInput() => SwitchActionMap(inputActions.PauseMenu, true);

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
}
