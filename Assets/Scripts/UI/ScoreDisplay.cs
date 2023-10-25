using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    static Text scoreText;
    /*Awakeで自身の初期化をして、Startで他の引数、コンポーネントの代入をすることで、NullReferenceExceptionを避ける*/
    private void Awake()
    {
        scoreText = GetComponent<Text>();
    }

    private void Start()
    {
        ScoreManager.Instance.ResetScore();
    }
    
    public static void UpdateText(int score)
    {
        scoreText.text = score.ToString();
    }

    public static void ScaleText(Vector3 targetScale) => scoreText.rectTransform.localScale = targetScale;
}
