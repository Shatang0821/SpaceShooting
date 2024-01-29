using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPool
{
    // �v���n�u�ւ̎Q�Ƃ��O������擾���邽�߂̃v���p�e�B
    public GameObject Prefab => prefab;

    // �T�C�Y���O������擾���邽�߂̃v���p�e�B
    public int Size => size;

    // ���s���̃L���[�̃T�C�Y���擾���邽�߂̃v���p�e�B
    public int RuntimeSize => queue.Count;

    [SerializeField]
    private GameObject prefab; // ���̃v�[���Ɋi�[����Q�[���I�u�W�F�N�g�̃v���n�u

    [SerializeField]
    private int size = 1; // �v�[���̏����T�C�Y

    // �Q�[���I�u�W�F�N�g��ێ����邽�߂̃L���[
    private Queue<GameObject> queue;

    // �Q�[���I�u�W�F�N�g���C���X�^���X�������Ƃ��̐e�I�u�W�F�N�g
    private Transform parent;
    #region �������֘A
    /// <summary>
    /// �L���[�̏��������A�w�肳�ꂽ���̃Q�[���I�u�W�F�N�g���L���[�ɒǉ�����
    /// </summary>
    /// <param name="parent">�e�I�u�W�F�N�g��Transform</param>
    public void Initialize(Transform parent)
    {
        //�L���[�̏�����
        queue = new Queue<GameObject>();
        //�e�I�u�W�F�N�g�𐶐����Ă���̉��ɃI�u�W�F�N�g�𐶐�����
        this.parent = parent;

        //�T�C�Y���̃I�u�W�F�N�g���L���[�ɓ����
        for (var i = 0; i < size; i++)
        {
            queue.Enqueue(Copy());
        }
    }

    /// <summary>
    /// �v���n�u����Q�[���I�u�W�F�N�g���쐬���A��A�N�e�B�u��Ԃɂ���
    /// </summary>
    private GameObject Copy()
    {
        //�쐬�����I�u�W�F�N�g��parent�̎q�I�u�W�F�N�g�ɂ���
        var copy = GameObject.Instantiate(prefab, parent);
        //������A�N�e�B�u���ɂ���
        copy.SetActive(false);
        //�쐬�����I�u�W�F�N�g��Ԃ�
        return copy;
    }
    #endregion

    #region �I�u�W�F�N�g�𐶐�
    /// <summary>
    /// ���p�\�ȃI�u�W�F�N�g���L���[����擾����B
    /// �����L���[����̏ꍇ�͐V�����I�u�W�F�N�g�𐶐�����B
    /// </summary>
    private GameObject AvailableObject()
    {
        GameObject availableobject = null;

        // �L���[����łȂ��A�擪�̃I�u�W�F�N�g����A�N�e�B�u�ȏꍇ
        if (queue.Count > 0 && !queue.Peek().activeSelf)
        {
            //Dequeue�̓L���[�̐擪����I�u�W�F�N�g�����o�����Ƃ��ł��邽��
            //�擪�̃I�u�W�F�N�g���g���Ă��鎞���o���Ȃ�
            availableobject = queue.Dequeue();
        }
        else
        {
            //���p�\�ȃI�u�W�F�N�g���Ȃ�����
            //�V�����I�u�W�F�N�g������āA�Ԃ�
            availableobject = Copy();
        }
        
        // �I�u�W�F�N�g���ĂуL���[�ɒǉ�����
        //�擪������o�����I�u�W�F�N�g�𖖂ɒǉ�����
        //�z�����邽��
        queue.Enqueue(availableobject);

        return availableobject;
    }
    #region �I�[�o�[���[�h
    /// <summary>
    /// ���p�\�ȃQ�[���I�u�W�F�N�g���擾���ăA�N�e�B�u������
    /// </summary>
    public GameObject preparedObject()
    {
        //�I�u�W�F�N�g�𐶐����鎞
        GameObject preparedobject = AvailableObject();
        //�A�N�e�B�u������
        preparedobject.SetActive(true);
        return preparedobject;
    }
    
    /// <summary>
    /// ����̈ʒu����ɐ���
    /// </summary>
    /// <param name="position">����̈ʒu</param>
    /// <returns></returns>
    public GameObject preparedObject(Vector3 position)
    {
        GameObject preparedobject = AvailableObject();

        preparedobject.SetActive(true);
        preparedobject.transform.position = position;

        return preparedobject;
    }

    /// <summary>
    /// ����̈ʒu�Ɖ�]����ɐ���
    /// </summary>
    /// <param name="position">����̈ʒu</param>
    /// <param name="rotation">����̉�]</param>
    /// <returns></returns>
    public GameObject preparedObject(Vector3 position,Quaternion rotation)
    {
        GameObject preparedobject = AvailableObject();

        preparedobject.SetActive(true);
        preparedobject.transform.position = position;
        preparedobject.transform.rotation = rotation;

        return preparedobject;
    }

    /// <summary>
    /// ����̈ʒu�Ɖ�]�Ɗg�����ɐ���
    /// </summary>
    /// <param name="position">����̈ʒu</param>
    /// <param name="rotation">����̉�]</param>
    /// <param name="localScale">����̊g��E�k��</param>
    /// <returns></returns>
    public GameObject preparedObject(Vector3 position, Quaternion rotation,Vector3 localScale)
    {
        GameObject preparedobject = AvailableObject();

        preparedobject.SetActive(true);
        preparedobject.transform.position = position;
        preparedobject.transform.rotation = rotation;
        preparedobject.transform.localScale = localScale;

        return preparedobject;
    }
    #endregion
    #endregion
}
