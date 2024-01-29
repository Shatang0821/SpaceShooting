using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class UIInput : Singleton<UIInput>
{
    [SerializeField] PlayerInput playerInput;

    /// <summary>
    /// UI�̓��͂��Ǘ�����R���|�[�l���g
    /// </summary>
    InputSystemUIInputModule UIInputModule;

    protected override void Awake()
    {
        base.Awake();
        UIInputModule = GetComponent<InputSystemUIInputModule>();
        UIInputModule.enabled = false;
    }

    /// <summary>
    /// UI��I��
    /// </summary>
    /// <param name="UIObject">�I���\��UI</param>
    /// <remarks>
    ///  UI��I������Ƃ�����InputSystemUIInputModule�L����
    /// </remarks>
    public void SelectUI(Selectable UIObject)
    {
        UIObject.Select();
        UIObject.OnSelect(null);
        UIInputModule.enabled = true;
    }

    /// <summary>
    /// ���ׂĂ̓��͂��~
    /// </summary>
    public void DisableAllUIInputs()
    {
        playerInput.DisableAllInputs();
        UIInputModule.enabled = false;
    }
}
