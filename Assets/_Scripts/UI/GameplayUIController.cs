using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIController : MonoBehaviour
{
    [Header("==== PLYAER INPUT ====")]
    [SerializeField] PlayerInput playerInput;       //�C���v�b�g

    [Header("==== AUDIO DATA ====")]
    [SerializeField] AudioData pauseSFX;            //��~���������Ƃ��̃T�E���h
    [SerializeField] AudioData unpauseSFX;          //��~�I�����������Ƃ��̃T�E���h

    [Header("==== CANVAS ====")]
    [SerializeField] Canvas hUDCanvas;              //HUD�L�����p�X
    [SerializeField] Canvas menusCanvas;            //���j���[�L�����p�X

    [Header("==== PLYAER INPUT ====")]
    [SerializeField] Button resumeButton;           //�Q�[���ɖ߂�{�^��

    [SerializeField] Button optionsButton;          //�I�v�V�����{�^��
        
    [SerializeField] Button mainMenuButton;         //���C�����j���[�̖߂�{�^��

    int buttonPressedParameterID = Animator.StringToHash("Pressed");//Pressed���n�b�V���l�̕ύX����

    private void OnEnable()
    {
        playerInput.onPause += Pause;
        playerInput.onUnpause += Unpause;

        //�����ɒǉ����邽��
        ButtonPressedBehavior.buttonFunctionTable.Add(resumeButton.gameObject.name, OnResumeButtonClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(optionsButton.gameObject.name, OnOptionsButtonClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(mainMenuButton.gameObject.name, OnMainMenuButtonClick);
    }

    private void OnDisable()
    {
        playerInput.onPause -= Pause;
        playerInput.onUnpause -= Unpause;

        ButtonPressedBehavior.buttonFunctionTable.Clear();
    }

    #region PAUSE
    /// <summary>
    /// �ꎞ��~����
    /// </summary>
    /// <remarks>
    /// <para>Canvas����,�Q�[����ԏ���,�Q�[�����ԃX�P�[������</para>
    /// <para>���͏���,���j���[�̏�������,�T�E���h����</para>
    /// </remarks>
    void Pause()
    {
        hUDCanvas.enabled = false;
        menusCanvas.enabled = true;

        GameManager.GameState = GameState.Paused;
        TimeController.Instance.Pause();

        playerInput.EnablePauseMenuiInput();
        playerInput.SwitchToDynamicUpdateMode();

        UIInput.Instance.SelectUI(resumeButton);
        AudioManager.Instance.PlaySFX(pauseSFX);
    }

    /// <summary>
    /// �Q�[���ĊJ
    /// </summary>
    /// <remarks>
    /// �A�j���[�V��������,�T�E���h����
    /// </remarks>
    public void Unpause()
    {
        resumeButton.Select();  //�{�^����I��������
        resumeButton.animator.SetTrigger(buttonPressedParameterID);//resumeButton�̉����ꂽ�A�j���[�V�������X�^�[�g������
        AudioManager.Instance.PlaySFX(unpauseSFX);
    }
    #endregion

    #region BUTTON CLICK
    /// <summary>
    /// �ĊJ�{�^��������������
    /// </summary>
    void OnResumeButtonClick()
    {
        hUDCanvas.enabled = true;                   //HUD��\��������
        menusCanvas.enabled = false;                //�ꎞ��~���j���[���\������
        GameManager.GameState = GameState.Playing;  //�Q�[����Ԃ�ς���
        TimeController.Instance.UnPause();
        playerInput.EnableGameplayInput();
        playerInput.SwitchToFixedUpdateMode();
    }

    /// <summary>
    /// �I�v�V�����{�^��
    /// </summary>
    void OnOptionsButtonClick()
    {
        // TODO
        UIInput.Instance.SelectUI(optionsButton);
        playerInput.EnablePauseMenuiInput();
    }

    /// <summary>
    /// ���C�����j���[�ɖ߂�{�^������
    /// </summary>
    void OnMainMenuButtonClick()
    {
        menusCanvas.enabled = false;                //���j���[���\��������
        SceneLoader.Instance.LoadMainMenuScene();   //���C�����j���[�ɖ߂�
    }
    #endregion


}
