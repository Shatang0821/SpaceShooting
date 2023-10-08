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
        //�V�[���̃��[�h���������Ă��邩�`�F�b�N
        var loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        loadingOperation.allowSceneActivation = false;  //���[�h�����̃V�[���̃A�N�e�B�u��Ԑ؂�ւ�

        transitionImage.gameObject.SetActive(true);

        //Fade out
        while (color.a < 1f)
        {
            //fade�^�C���ɂ���Ď����I�ɑ����Z����Clamp01 0�`1�Ԃɐ����ł���
            color.a = Mathf.Clamp01( color.a += Time.unscaledDeltaTime / fadeTime);
            transitionImage.color = color;

            yield return null;
        }

        loadingOperation.allowSceneActivation = true;

        //Fade in
        while (color.a > 0f)
        {
            //fade�^�C���ɂ���Ď����I�ɑ����Z����Clamp01 0�`1�Ԃɐ����ł���
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
