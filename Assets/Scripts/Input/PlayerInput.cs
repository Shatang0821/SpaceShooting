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

    //�������̂��߂̃C�x���g�ł�
    public event UnityAction onDodge = delegate { };

    //�����̂��߂̃C�x���g�ł�
    public event UnityAction onOverdrive = delegate { };

    //�ꎞ��~�̂��߂̃C�x���g�ł�
    public event UnityAction onPause = delegate { };

    //�J�n�̂��߂̃C�x���g�ł�
    public event UnityAction onUnpause = delegate { };

    public event UnityAction onLaunchMissile = delegate { };

    public event UnityAction onConfirmGameOver = delegate { };

    // �V����Input System�̃A�N�V�����ւ̎Q�ƁB
    InputActions inputActions;

    // �L���ɂ��ꂽ���ɌĂ΂�郁�\�b�h�B
    void OnEnable()
    {
        inputActions = new InputActions();

        inputActions.Gameplay.SetCallbacks(this);
        inputActions.PauseMenu.SetCallbacks(this);
        inputActions.GameOverScreen.SetCallbacks(this);

    }

    // �����ɂ��ꂽ���ɌĂ΂�郁�\�b�h�B
    void OnDisable()
    {
        DisableAllInputs();
    }

    /// <summary>
    /// �L��actionmap��ς��
    /// </summary>
    /// <param name="actionMap">�ς�����actionMap</param>
    /// <param name="isUIInput">UI�̑I����</param>
    void SwitchActionMap(InputActionMap actionMap, bool isUIInput)
    {
        inputActions.Disable();
        actionMap.Enable();

        if(isUIInput)
        {
            Cursor.visible = true;                     // �}�E�X�J�[�\�������ɂ��܂��B
            Cursor.lockState = CursorLockMode.None;    // �}�E�X�J�[�\�������b�N���Ȃ��B
        }
        else
        {
            Cursor.visible = false;                     // �}�E�X�J�[�\����s���ɂ��܂��B
            Cursor.lockState = CursorLockMode.Locked;   // �}�E�X�J�[�\�������b�N����B
        }
    }

    /*
        Time.timeScale��0�ɂȂ�Ƃ�InputSystem��
        UpdateMode�� In Fixed Update��������͂������ɂȂ�
        �����h�����߁A�ꎞ��~�̎���UpdateMode��DynamicUpdate�ς���
     */
    /// <summary>
    /// ���͂�ProcessEventsInDynamicUpdate�ɕς���
    /// </summary>
    public void SwitchToDynamicUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;

    /// <summary>
    /// ���͂�ProcessEventsInFixedUpdate�ɕς���
    /// </summary>
    public void SwitchToFixedUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;

    /// <summary>
    /// ���͂𖳌�������
    /// </summary>
    public void DisableAllInputs()�@=> inputActions.Disable();

    /// <summary>
    /// �Q�[�����ŃL�����N�^�[�𑀍삷�鎞�ɓ��͂�L�������郁�\�b�h�B
    /// </summary>
    public void EnableGameplayInput() => SwitchActionMap(inputActions.Gameplay, false);

    /// <summary>
    /// �ꎞ��~��ʓ��̓��͂�L�������郁�\�b�h
    /// </summary>
    public void EnablePauseMenuiInput() => SwitchActionMap(inputActions.PauseMenu, true);

    /// <summary>
    /// �Q�[���I�[�o�[���̓���
    /// </summary>
    public void EnableGameOverScreenInput() => SwitchActionMap(inputActions.GameOverScreen, true);

    // �ړ��A�N�V�������g���K�[���ꂽ���ɌĂ΂�郁�\�b�h�B
    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            EventCenter.TriggerEvent(EventNames.Move, context.ReadValue<Vector2>());
        }

        if(context.canceled)
        {
            EventCenter.TriggerEvent(EventNames.StopMove);
        }
    }

    // ���˃A�N�V�������g���K�[���ꂽ���ɌĂ΂�郁�\�b�h�B
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            EventCenter.TriggerEvent(EventNames.Fire);
        }

        if (context.canceled)
        {
            EventCenter.TriggerEvent(EventNames.StopFire);
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
       if(context.performed)
        {
            EventCenter.TriggerEvent(EventNames.Dodge);
            //onDodge.Invoke();
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
            EventCenter.TriggerEvent(EventNames.LaunchMissile);
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
