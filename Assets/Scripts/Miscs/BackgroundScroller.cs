using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField]Vector2 scrollVelocity;//public������K�v�Ȃ��̂�
                                           //SerializeField�g���ăC���X�y�N�^�[�ŕҏW�\�ɂ���

    Material material;
    // Start is called before the first frame update
    void Awake()
    {
        material =GetComponent<Renderer>().material;
    }

    IEnumerator Start()
    {
        while(GameManager.GameState != GameState.GameOver)
        {
            material.mainTextureOffset += scrollVelocity * Time.deltaTime;

            yield return null;
        }    
    }
}
