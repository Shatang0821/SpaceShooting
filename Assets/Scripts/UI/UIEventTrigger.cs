using UnityEngine;
using UnityEngine.EventSystems;
                                             
public class UIEventTrigger : 
    MonoBehaviour, 
    IPointerEnterHandler,   //�}�E�X�J�[�\�����{�^���͈͓��ɂ��邩���`�F�b�N
    IPointerDownHandler,    //�}�E�X�Œ�o������Ăяo��
    ISelectHandler,         //�L�[�{�[�h��Q�[���p�b�h�őI��������
    ISubmitHandler          //�L�[�{�[�h��Q�[���p�b�h�ȂǂŒ�o������Ăяo��
{
    [SerializeField] AudioData selectSFX;
    [SerializeField] AudioData submitSFX;


    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX(selectSFX);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX(submitSFX);
    }

    public void OnSelect(BaseEventData eventData)
    {
        AudioManager.Instance.PlaySFX(selectSFX);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        AudioManager.Instance.PlaySFX(submitSFX);
    }
}
