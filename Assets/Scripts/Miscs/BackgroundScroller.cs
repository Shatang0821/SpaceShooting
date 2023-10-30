using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField]Vector2 scrollVelocity;//public化する必要ないので
                                           //SerializeField使ってインスペクターで編集可能にする

    Material material;
    // Start is called before the first frame update
    void Awake()
    {
        material =GetComponent<Renderer>().material;
    }

    IEnumerator Start()
    {
        while(true)
        {
            material.mainTextureOffset += scrollVelocity * Time.deltaTime;

            yield return null;
        }    
    }
}
