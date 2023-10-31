using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [SerializeField] Button buttonStartGame;

    private void OnEnable()
    {
        buttonStartGame.onClick.AddListener(OnStartGameButtonClick);
    }

    private void OnDisable()
    {
        buttonStartGame.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        Time.timeScale = 1f;
        GameManager.GameState = GameState.Playing;
    }
    void OnStartGameButtonClick()
    {
        SceneLoader.Instance.LoadGamePlayScene();
    }
}
