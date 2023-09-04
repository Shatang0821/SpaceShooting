using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Pool
{
    [SerializeField]GameObject prefab;

    [SerializeField]int size = 1;

    //�I�u�W�F�N�g�v�[��
    Queue<GameObject> queue;

    /// <summary>
    /// �L���[�̏�����
    /// </summary>
    public void Initialize()
    {
        queue = new Queue<GameObject>();//������

        for(var i = 0;i<size;i++)
        {
            queue.Enqueue(Copy());//�I�u�W�F�N�g�𖖔��ɓ����
        }
    }

    /// <summary>
    /// �v���n�u�����O�ɍ쐬���Ĕ�A�N�e�B�u��Ԃɂ���
    /// </summary>
    GameObject Copy()
    {
        var copy = GameObject.Instantiate(prefab);
        copy.SetActive(false);

        return copy;
    }

    /// <summary>
    /// �L���[�̐擪������o��
    /// </summary>
    /// <returns></returns>
    GameObject AvailableObject()
    {
        GameObject availableobject = null;//������

        if(queue.Count > 0 && queue.Peek().activeSelf)//�L���[�̐擪��Ԃ��A�o������Ȃ���
        {
            availableobject = queue.Dequeue();
        }
        else
        {
            availableobject = Copy();
        }

        queue.Enqueue(availableobject);

        return availableobject;
    }

    /// <summary>
    /// ���p�\�ȃI�u�W�F�N�g��Ԃ�
    /// </summary>
    /// <returns></returns>
    public GameObject preparedObject()
    {
        GameObject preparedobject = AvailableObject();

        preparedobject.SetActive(true);

        return preparedobject;
    }

    /// <summary>
    /// ����̈ʒu����ɐ���
    /// </summary>
    /// <param name="position"></param>
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
    /// <param name="position"></param>
    /// <param name="rotation"></param>
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
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="localScale"></param>
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


}
