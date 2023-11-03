using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoringUIController : MonoBehaviour
{
    [Header("==== SCORING SCREEN ====")]
    [SerializeField] Canvas scoringScreenCanvas;

    [SerializeField] Text playerScoreText;
    [SerializeField] Button buttonMainMenu;

    [SerializeField] Transform highScoreLeaderboardContainer;

    [Header("==== HIGHT SCORE SCREEN ====")]
    [SerializeField] Canvas newHighScoreScreenCanvas;
    [SerializeField] Button buttonCancel;
    [SerializeField] Button buttonSubmit;
    [SerializeField] InputField playerNameInputField;

    private void Start()
    {
        //�}�E�X�J�[�\���\��
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (ScoreManager.Instance.HasNewHighScore)
        {
            ShowNewHighScoreScreen();
        }
        else
        {
            ShowScoringScreen();
        }

        ButtonPressedBehavior.buttonFunctionTable.Add(buttonMainMenu.gameObject.name, OnButtonMainMenuClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonSubmit.gameObject.name, OnButtonSubmitClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonCancel.gameObject.name, HideNewHighScoreScreen);

        GameManager.GameState = GameState.Scoring;
    }

    /// <summary>
    /// ���O���͉�ʂ�\������
    /// </summary>
    void ShowNewHighScoreScreen()
    {
        newHighScoreScreenCanvas.enabled = true;
        UIInput.Instance.SelectUI(buttonCancel);

    }

    /// <summary>
    /// ���O���͉�ʂ����
    /// </summary>
    void HideNewHighScoreScreen()
    {
        newHighScoreScreenCanvas.enabled = false;
        ScoreManager.Instance.SavePlayerScoreData();
        ShowScoringScreen();
    }

    private void OnDisable()
    {
        ButtonPressedBehavior.buttonFunctionTable.Clear();
    }

    /// <summary>
    /// �X�R�A��ʂ�\������
    /// </summary>
    void ShowScoringScreen()
    {
        scoringScreenCanvas.enabled = true;
        playerScoreText.text = ScoreManager.Instance.Score.ToString();
        UIInput.Instance.SelectUI(buttonMainMenu);
        UpdateHighScoreLeaderboard();
    }

    /// <summary>
    /// �X�R�A�����L���O���A�b�v�f�[�g
    /// </summary>
    void UpdateHighScoreLeaderboard()
    {
        var playerScoreList = ScoreManager.Instance.LoadPlayerScoreData().list;

        for(int i = 0;i<highScoreLeaderboardContainer.childCount;i++)
        {
            var child = highScoreLeaderboardContainer.GetChild(i);

            child.Find("Rank").GetComponent<Text>().text = (i + 1).ToString();
            child.Find("Score").GetComponent<Text>().text = playerScoreList[i].score.ToString();
            child.Find("Name").GetComponent<Text>().text = playerScoreList[i].playerName;

        }
    }

    /// <summary>
    /// ���C�����j���[�ɖ߂�
    /// </summary>
    void OnButtonMainMenuClicked()
    {
        scoringScreenCanvas.enabled = false;
        SceneLoader.Instance.LoadMainMenuScene();
    }

    /// <summary>
    /// �V�������O�ƃX�R�A���o����
    /// </summary>
    void OnButtonSubmitClicked()
    {
        if(!string.IsNullOrEmpty(playerNameInputField.text))
        {
            ScoreManager.Instance.SetPlayerName(playerNameInputField.text);
        }
        HideNewHighScoreScreen();
    }

    
}
