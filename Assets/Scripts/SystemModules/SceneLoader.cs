using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : PersistentSingleton<SceneLoader>
{
    [SerializeField] UnityEngine.UI.Image transitionImage;
    [SerializeField] float fadeTime = 3.5f;

    Color color;

    const string GAMEPLAY = "Gameplay";

    void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator LoadCoroutine(string sceneName)
    {
        //シーンのロードを完了しているかチェック
        var loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        loadingOperation.allowSceneActivation = false;  //ロード完了のシーンのアクティブ状態切り替え

        transitionImage.gameObject.SetActive(true);

        //Fade out
        while (color.a < 1f)
        {
            //fadeタイムによって自動的に足し算するClamp01 0～1間に制限できる
            color.a = Mathf.Clamp01( color.a += Time.unscaledDeltaTime / fadeTime);
            transitionImage.color = color;

            yield return null;
        }

        loadingOperation.allowSceneActivation = true;

        //Fade in
        while (color.a > 0f)
        {
            //fadeタイムによって自動的に足し算するClamp01 0～1間に制限できる
            color.a = Mathf.Clamp01(color.a -= Time.unscaledDeltaTime / fadeTime);
            transitionImage.color = color;

            yield return null;
        }
        transitionImage.gameObject.SetActive(false);
    }

    public void LoadGamePlayScene()
    {
        StartCoroutine(LoadCoroutine(GAMEPLAY));
    }
}
