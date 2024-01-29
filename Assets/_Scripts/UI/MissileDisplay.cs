using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissileDisplay : MonoBehaviour
{
    static Text amountText;

    static Image cooldownImage;

    static Animator animator;
    /*Awake�Ŏ��g�̏����������āAStart�ő��̈����A�R���|�[�l���g�̑�������邱�ƂŁANullReferenceException�������*/
    private void Awake()
    {
        amountText = transform.Find("Amount Text").GetComponent<Text>();
        cooldownImage = transform.Find("Cooldown Image").GetComponent<Image>();
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// �~�T�C���e�L�X�g�A�b�v�f�[�g
    /// </summary>
    /// <param name="amount"></param>
    public static void UpdateAmountText(int amount) => amountText.text = amount.ToString();
    /// <summary>
    /// fill�l�̕ύX
    /// </summary>
    /// <param name="fillAmount"></param>
    public static void UpdateCooldownImage(float fillAmount) => cooldownImage.fillAmount = fillAmount;
    //�g�p�s�̉摜shake
    public static void ShakeIcon() => animator.SetTrigger("NoneAmount");
}
