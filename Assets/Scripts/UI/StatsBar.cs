using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour
{ 
    //�摜
    [SerializeField] Image fillImageBack;
    [SerializeField] Image fillImageFront;

    //�x��Ă���t�B���l��ω�������
    [SerializeField] bool delayFille = true;

    //fillDelay���Ԃ̌ォ��ω��J�n
    [SerializeField] float fillDelay = 0.5f;
    //�ω����x
    [SerializeField] float fillSpeed = 0.1f;

    //�v�Z�p
    float currentFillAmout;

    //�ڕW�t�B���l
    protected float targetFillAmout;

    //���݃t�B���l
    float previousFillAmount;

    float t;

    WaitForSeconds waitForDelayFill;

    Coroutine bufferedFillingCoroutine;

    Canvas canvas;

    private void Awake()
    {
        //Canvas�����݂��Ă���Ƃ������ݒ肷��
        if (TryGetComponent<Canvas>(out Canvas canvas))
        {
            canvas.worldCamera = Camera.main;
        }
        waitForDelayFill = new WaitForSeconds(fillDelay);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public virtual void Initialize(float currentValue,float maxValue)
    {
        //�Ⴆ�� ���̎��Ńp�[�Z���g�ŕ\���܂�
        //100/100 = 1�܂�100%
        currentFillAmout = currentValue / maxValue;
        targetFillAmout = currentFillAmout;
        fillImageBack.fillAmount = currentFillAmout;
        fillImageFront.fillAmount = currentFillAmout;
    }

    public void UpdateStats(float currentValue, float maxValue)
    {
        //�p�[�Z���g���ɒ���
        targetFillAmout = currentValue / maxValue;

        //�R���[�`�������s���Ă��鎞�~�߂�
        if(bufferedFillingCoroutine != null)
        {
            StopCoroutine(bufferedFillingCoroutine);
        }
        // �ڕW�l���傫���ꍇ
        if(currentFillAmout > targetFillAmout)
        {
            //�O�̃t�B���l��ڕW�t�B���l�ɒ���
            fillImageFront.fillAmount = targetFillAmout;
            // �R���[�`�����g���ė��摜�������ω�������
            bufferedFillingCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageBack));

            return;
        }
        // �ڕW�l��菬�����ꍇ
        if(currentFillAmout < targetFillAmout)
        {
            // ���̃t�B���l��ڕW�t�B���l�ɒ���
            fillImageBack.fillAmount = targetFillAmout;
            // �R���[�`�����g���đO�摜�������ω�������
            bufferedFillingCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageFront));
        }

    }

    /// <summary>
    /// �t�B���l��ω�������R���[�`��
    /// </summary>
    /// <param name="image">�ω�������摜</param>
    /// <returns></returns>
    protected virtual IEnumerator BufferedFillingCoroutine(Image image)
    {
        //�ҋ@���Ԃ���Ȃ�
        if(delayFille)
        {
            //�҂�
            yield return waitForDelayFill;
        }
        //���̃t�B���l���X�V���āA�����r���ŃR���[�`�����~�߂��ꍇ�ł�
        //�������t�B���l����v�Z���n�߂�
        previousFillAmount = currentFillAmout;
        t = 0;

        while(t < 1f)
        {
            t += Time.deltaTime * fillSpeed;
            //lerp�g�Đ��`��Ԃ�����
            currentFillAmout = Mathf.Lerp(previousFillAmount, targetFillAmout, t);
            image.fillAmount = currentFillAmout;

            yield return null;
        }
        
    }

}
