using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomImage : MonoBehaviour
{
    [SerializeField] Sprite[] backGroundsImages;

    Image image;

    private void Start()
    {
        ShowRandomBackground();
    }

    void ShowRandomBackground()
    {
        image = GetComponent<Image>();
        image.sprite = backGroundsImages[Random.Range(0, backGroundsImages.Length)];
    }
}
