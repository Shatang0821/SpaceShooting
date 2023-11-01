using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class UIInput : Singleton<UIInput>
{
    [SerializeField] PlayerInput playerInput;

    /// <summary>
    /// UIの入力を管理するコンポーネント
    /// </summary>
    InputSystemUIInputModule UIInputModule;

    protected override void Awake()
    {
        base.Awake();
        UIInputModule = GetComponent<InputSystemUIInputModule>();
        UIInputModule.enabled = false;
    }

    /// <summary>
    /// UIを選択
    /// </summary>
    /// <param name="UIObject">選択可能のUI</param>
    /// <remarks>
    ///  UIを選択するときだけInputSystemUIInputModule有効化
    /// </remarks>
    public void SelectUI(Selectable UIObject)
    {
        UIObject.Select();
        UIObject.OnSelect(null);
        UIInputModule.enabled = true;
    }

    /// <summary>
    /// すべての入力を停止
    /// </summary>
    public void DisableAllUIInputs()
    {
        playerInput.DisableAllInputs();
        UIInputModule.enabled = false;
    }
}
