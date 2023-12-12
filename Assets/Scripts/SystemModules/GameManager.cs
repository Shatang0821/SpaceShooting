using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : PersistentSingleton<GameManager>
{ 
    //プレイヤーが死亡したときイベントを実行する
    public static System.Action onGameOver;
    
    public static GameState GameState { get => Instance.gameState; set => Instance.gameState = value; }

    [SerializeField] GameState gameState = GameState.Playing;

    private void Start()
    {
        Application.targetFrameRate =60;
    }
}

public enum GameState
{
    Playing,
    Paused,
    GameOver,
    Scoring
}
