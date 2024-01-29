using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissileDisplay : MonoBehaviour
{
    static Text amountText;

    static Image cooldownImage;

    static Animator animator;
    /*Awakeで自身の初期化をして、Startで他の引数、コンポーネントの代入をすることで、NullReferenceExceptionを避ける*/
    private void Awake()
    {
        amountText = transform.Find("Amount Text").GetComponent<Text>();
        cooldownImage = transform.Find("Cooldown Image").GetComponent<Image>();
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// ミサイルテキストアップデート
    /// </summary>
    /// <param name="amount"></param>
    public static void UpdateAmountText(int amount) => amountText.text = amount.ToString();
    /// <summary>
    /// fill値の変更
    /// </summary>
    /// <param name="fillAmount"></param>
    public static void UpdateCooldownImage(float fillAmount) => cooldownImage.fillAmount = fillAmount;
    //使用不可の画像shake
    public static void ShakeIcon() => animator.SetTrigger("NoneAmount");
}
