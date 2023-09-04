using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Pool
{
    [SerializeField]GameObject prefab;

    [SerializeField]int size = 1;

    //オブジェクトプール
    Queue<GameObject> queue;

    /// <summary>
    /// キューの初期化
    /// </summary>
    public void Initialize()
    {
        queue = new Queue<GameObject>();//初期化

        for(var i = 0;i<size;i++)
        {
            queue.Enqueue(Copy());//オブジェクトを末尾に入れる
        }
    }

    /// <summary>
    /// プレハブを事前に作成して非アクティブ状態にする
    /// </summary>
    GameObject Copy()
    {
        var copy = GameObject.Instantiate(prefab);
        copy.SetActive(false);

        return copy;
    }

    /// <summary>
    /// キューの先頭から取り出す
    /// </summary>
    /// <returns></returns>
    GameObject AvailableObject()
    {
        GameObject availableobject = null;//初期化

        if(queue.Count > 0 && queue.Peek().activeSelf)//キューの先頭を返す、出すじゃなくて
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
    /// 利用可能なオブジェクトを返す
    /// </summary>
    /// <returns></returns>
    public GameObject preparedObject()
    {
        GameObject preparedobject = AvailableObject();

        preparedobject.SetActive(true);

        return preparedobject;
    }

    /// <summary>
    /// 特定の位置を基に生成
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
    /// 特定の位置と回転を基に生成
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
    /// 特定の位置と回転と拡大を基に生成
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
