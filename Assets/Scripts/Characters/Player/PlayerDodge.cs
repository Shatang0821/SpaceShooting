using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodge : MonoBehaviour
{
    #region PlayerDodge
    [Header("----- DODGE -----")]

    [SerializeField] AudioData dodgeSFX;//���ʉ�

    [Tooltip("����̓L�����N�^�[�̃G�l���M�[���՗ʂł��B")]
    [SerializeField, Range(0, 100)] int dodgeEnergyCost = 25;

    [SerializeField] float maxRoll = 720f;//�ő��]�p�x

    [SerializeField] float rollSpeed = 360f;//��]���x
    [SerializeField] Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);//�X�P�[���ω�

    bool isDodging = false; //�_�b�W�@�]�����킷

    float currentRoll;//���݉�]�l

    float dodgeDuration;//�_�b�W��������

    readonly float slowMotionDuration = 1f;//�o���b�g�^�C��slowout����
    #endregion

    new Collider2D collider;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();

        dodgeDuration = maxRoll / rollSpeed;//�ő��]�p�x/��]���x = ����
    }

    private void OnEnable()
    {
        EventCenter.Subscribe(EventNames.Dodge, Dodge);
    }

    private void OnDisable()
    {
        EventCenter.Unsubscribe(EventNames.Dodge, Dodge);
    }
    #region DODGE
    void Dodge()
    {
        //�����_�b�W���ł���Ώ��������Ȃ��܂��A�G�l���M�[������Ȃ��Ƃ������l
        if (isDodging || !PlayerEnergy.Instance.IsEnough(dodgeEnergyCost)) return;
        //�����łȂ���Ώ����ɓ���
        StartCoroutine(nameof(DodgeCoroutine));
        // Change Player's scale
    }

    /// <summary>
    /// �_�b�W���s�R���[�`��
    /// </summary>
    /// <returns></returns>
    IEnumerator DodgeCoroutine()
    {
        isDodging = true;
        AudioManager.Instance.PlayRandomSFX(dodgeSFX);
        // �G�l���M�[������
        PlayerEnergy.Instance.Use(dodgeEnergyCost);

        //���̂ƒʉ߂ł���悤�ɂ���
        collider.isTrigger = true;

        // ������]��0�ɂ���
        currentRoll = 0f;

        //�o���b�g�^�C�����J�n
        TimeController.Instance.BulletTime(slowMotionDuration, slowMotionDuration);

        //���݉�]���ő��]��菬�����ꍇ���[�v������
        while (currentRoll < maxRoll)
        {
            currentRoll += rollSpeed * Time.deltaTime;
            //��]������
            transform.rotation = Quaternion.AngleAxis(currentRoll, Vector3.right);

            //�x�W�F�Ȑ����g���ē������Ȃ߂炩�ɂ���
            //scale��1����0.5�܂�1�ɖ߂�悤�ɂ���
            transform.localScale = BezierCurve.QuadraticPoint(Vector3.one, Vector3.one, dodgeScale, currentRoll / maxRoll);
            yield return null;
        }

        collider.isTrigger = false;
        isDodging = false;
    }
    #endregion

}
