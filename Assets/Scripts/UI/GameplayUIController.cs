using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIController : MonoBehaviour
{
    [Header("==== PLYAER INPUT ====")]
    [SerializeField] PlayerInput playerInput;       //インプット

    [Header("==== AUDIO DATA ====")]
    [SerializeField] AudioData pauseSFX;            //停止を押したときのサウンド
    [SerializeField] AudioData unpauseSFX;          //停止終了を押したときのサウンド

    [Header("==== CANVAS ====")]
    [SerializeField] Canvas hUDCanvas;              //HUDキャンパス
    [SerializeField] Canvas menusCanvas;            //メニューキャンパス

    [Header("==== PLYAER INPUT ====")]
    [SerializeField] Button resumeButton;           //ゲームに戻るボタン

    [SerializeField] Button optionsButton;          //オプションボタン
        
    [SerializeField] Button mainMenuButton;         //メインメニューの戻るボタン

    int buttonPressedParameterID = Animator.StringToHash("Pressed");//Pressedをハッシュ値の変更する

    private void OnEnable()
    {
        playerInput.onPause += Pause;
        playerInput.onUnpause += Unpause;

        //辞書に追加するため
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
    /// 一時停止処理
    /// </summary>
    /// <remarks>
    /// <para>Canvas処理,ゲーム状態処理,ゲーム時間スケール処理</para>
    /// <para>入力処理,メニューの初期処理,サウンド処理</para>
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
    /// ゲーム再開
    /// </summary>
    /// <remarks>
    /// アニメーション処理,サウンド処理
    /// </remarks>
    public void Unpause()
    {
        resumeButton.Select();  //ボタンを選択させる
        resumeButton.animator.SetTrigger(buttonPressedParameterID);//resumeButtonの押されたアニメーションをスタートさせる
        AudioManager.Instance.PlaySFX(unpauseSFX);
    }
    #endregion

    #region BUTTON CLICK
    /// <summary>
    /// 再開ボタンが押した処理
    /// </summary>
    void OnResumeButtonClick()
    {
        hUDCanvas.enabled = true;                   //HUDを表示させる
        menusCanvas.enabled = false;                //一時停止メニューを非表示する
        GameManager.GameState = GameState.Playing;  //ゲーム状態を変える
        TimeController.Instance.UnPause();
        playerInput.EnableGameplayInput();
        playerInput.SwitchToFixedUpdateMode();
    }

    /// <summary>
    /// オプションボタン
    /// </summary>
    void OnOptionsButtonClick()
    {
        // TODO
        UIInput.Instance.SelectUI(optionsButton);
        playerInput.EnablePauseMenuiInput();
    }

    /// <summary>
    /// メインメニューに戻るボタン処理
    /// </summary>
    void OnMainMenuButtonClick()
    {
        menusCanvas.enabled = false;                //メニューを非表示させる
        SceneLoader.Instance.LoadMainMenuScene();   //メインメニューに戻る
    }
    #endregion


}
