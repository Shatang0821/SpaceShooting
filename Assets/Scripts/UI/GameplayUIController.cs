using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIController : MonoBehaviour
{
    [Header("==== PLYAER INPUT ====")]
    [SerializeField] PlayerInput playerInput;       //インプット

    [Header("==== AUDIO DATA ====")]
    [SerializeField] AudioData pauseSFX;
    [SerializeField] AudioData unpauseSFX;

    [Header("==== CANVAS ====")]
    [SerializeField] Canvas hUDCanvas;              //HUDキャンパス
    [SerializeField] Canvas menusCanvas;            //メニューキャンパス

    [Header("==== PLYAER INPUT ====")]
    [SerializeField] Button resumeButton;

    [SerializeField] Button optionsButton;

    [SerializeField] Button mainMenuButton;

    int buttonPressedParameterID = Animator.StringToHash("Pressed");

    private void OnEnable()
    {
        playerInput.onPause += Pause;
        playerInput.onUnpause += Unpause;

        ButtonPressedBehavior.buttonFunctionTable.Add(resumeButton.gameObject.name, OnResumeButtonClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(optionsButton.gameObject.name, OnOptionsButtonClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(mainMenuButton.gameObject.name, OnMainMenuButtonClick);
    }

    private void OnDisable()
    {
        playerInput.onPause -= Pause;
        playerInput.onUnpause -= Unpause;

        resumeButton.onClick.RemoveAllListeners();
        optionsButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.RemoveAllListeners();

        ButtonPressedBehavior.buttonFunctionTable.Clear();
    }

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

    public void Unpause()
    {
        resumeButton.Select();
        resumeButton.animator.SetTrigger(buttonPressedParameterID);
        AudioManager.Instance.PlaySFX(unpauseSFX);
    }

    void OnResumeButtonClick()
    {
        hUDCanvas.enabled = true;
        menusCanvas.enabled = false;
        GameManager.GameState = GameState.Playing;
        TimeController.Instance.UnPause();
        playerInput.EnableGameplayInput();
        playerInput.SwitchToFixedUpdateMode();
    }

    void OnOptionsButtonClick()
    {
        // TODO
        UIInput.Instance.SelectUI(optionsButton);
        playerInput.EnablePauseMenuiInput();
    }

    void OnMainMenuButtonClick()
    {
        menusCanvas.enabled = false;
        SceneLoader.Instance.LoadMainMenuScene();
    }


}
