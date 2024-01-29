using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : Singleton<TimeController>
{
    /// <summary>
    /// ��Ԓx���Ȃ鎞��
    /// </summary>
    [SerializeField,Range(0,1f)] float bulletTimeScale = 0.1f;  //�o���b�g�^�C��

    float defaultFixedDeltaTime;                                //����FixedDeltaTime;

    float timeScaleBeforePause;//�o���b�g�^�C������Pause�ɓ���A�Q�[���ɖ߂鎞���̃X�P�[���ɖ߂邽��

    float t;                                                    //lerp�̑�O����
    protected override void Awake()
    {
        base.Awake();
        defaultFixedDeltaTime = Time.fixedDeltaTime;
    }

    /// <summary>
    /// timeScale��0�ɂ���
    /// </summary>
    public void Pause()
    {
        timeScaleBeforePause = Time.timeScale;
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Pause�J�n����timeScale�ɖ߂�
    /// </summary>
    public void UnPause()
    {
        Time.timeScale = timeScaleBeforePause;
    }

    #region BULLET TIME
    /// <summary>
    /// �o���b�g�^�C���X�^�[�g������
    /// </summary>
    /// <param name="duration">���Ԓ���</param>
    public void BulletTime(float duration)
    {
        Time.timeScale = bulletTimeScale;
        StartCoroutine(SlowOutCoroutine(duration));
    }

    /// <summary>
    /// in out��������
    /// </summary>
    /// <param name="inDuration">In��������</param>
    /// <param name="outDuration">Out��������</param>
    public void BulletTime(float inDuration,float outDuration)
    {
        StartCoroutine(SlowInAndOutCoroutine(inDuration,outDuration));
    }

    public void BulletTime(float inDuration, float keepingDuration, float outDuration)
    {
        StartCoroutine(SlowInKeepAndOutDuration(inDuration,keepingDuration,outDuration));
    }


    public void SlowIn(float duration)
    {
        StartCoroutine(SlowInCoroutine(duration));
    }

    public void SlowOut(float duration)
    {
        StartCoroutine(SlowOutCoroutine(duration));
    }

    IEnumerator SlowInKeepAndOutDuration(float inDuration,float keepingDuration, float outDuration)
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));

        yield return new WaitForSecondsRealtime(keepingDuration);

        StartCoroutine(SlowOutCoroutine(outDuration));
    }

    /// <summary>
    /// �o���b�g�^�C���X�^�[�g
    /// </summary>
    /// <param name="inDuration">In��������</param>
    /// <param name="outDuration">Out��������</param>
    /// <returns></returns>
    IEnumerator SlowInAndOutCoroutine(float inDuration,float outDuration)
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));

        StartCoroutine(SlowOutCoroutine(outDuration));
    }


    /// <summary>
    /// �������n�߂�
    /// </summary>
    /// <param name="duration">��������</param>
    /// <returns></returns>
    IEnumerator SlowInCoroutine(float duration)
    {
        t = 0f;

        while (t < 1f)
        {
            if(GameManager.GameState != GameState.Paused )
            {
                //Time.deltaTime�� timescale�ς��Ƃ����ς�邩��g���Ȃ�
                //Time.unscaledDeltaTime��scale�̕ύX�ɂ���ĉe�����o�Ȃ�
                //���Ԍo�߂�t��0����1�ɏ��X�ɑ���������
                t += Time.unscaledDeltaTime / duration;
                //����t�̍ő�l��1�Ȃ̂ł���duration��1���傫��������������Ōv�Z����
                Time.timeScale = Mathf.Lerp(1f, bulletTimeScale, t);
                //�Q�[���̎��Ԃ̃X�P�[���ɍ��킹�ĕ����̍X�V�Ԋu�𒲐����邽��
                //����ɂ��scale���ω����Ă������v�Z���������s����
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
            }
            yield return null;
        }
    }

    /// <summary>
    /// �o���b�g�^�C�����������I��点��
    /// </summary>
    /// <param name="duration">���Ԓ���</param>
    /// <returns></returns>
    IEnumerator SlowOutCoroutine(float duration)
    {
        t = 0f;

        while(t < 1f)
        {
            if(GameManager.GameState != GameState.Paused)
            {
                //Time.deltaTime�� timescale�ς��Ƃ����ς�邩��g���Ȃ�
                t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(bulletTimeScale, 1f, t);
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
            }
            yield return null;
        }
    }
    #endregion
}
