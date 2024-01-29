using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPool
{
    // プレハブへの参照を外部から取得するためのプロパティ
    public GameObject Prefab => prefab;

    // サイズを外部から取得するためのプロパティ
    public int Size => size;

    // 実行時のキューのサイズを取得するためのプロパティ
    public int RuntimeSize => queue.Count;

    [SerializeField]
    private GameObject prefab; // このプールに格納するゲームオブジェクトのプレハブ

    [SerializeField]
    private int size = 1; // プールの初期サイズ

    // ゲームオブジェクトを保持するためのキュー
    private Queue<GameObject> queue;

    // ゲームオブジェクトがインスタンス化されるときの親オブジェクト
    private Transform parent;
    #region 初期化関連
    /// <summary>
    /// キューの初期化し、指定された数のゲームオブジェクトをキューに追加する
    /// </summary>
    /// <param name="parent">親オブジェクトのTransform</param>
    public void Initialize(Transform parent)
    {
        //キューの初期化
        queue = new Queue<GameObject>();
        //親オブジェクトを生成してそれの下にオブジェクトを生成する
        this.parent = parent;

        //サイズ分のオブジェクトをキューに入れる
        for (var i = 0; i < size; i++)
        {
            queue.Enqueue(Copy());
        }
    }

    /// <summary>
    /// プレハブからゲームオブジェクトを作成し、非アクティブ状態にする
    /// </summary>
    private GameObject Copy()
    {
        //作成したオブジェクトをparentの子オブジェクトにする
        var copy = GameObject.Instantiate(prefab, parent);
        //初期非アクティブ化にする
        copy.SetActive(false);
        //作成したオブジェクトを返す
        return copy;
    }
    #endregion

    #region オブジェクトを生成
    /// <summary>
    /// 利用可能なオブジェクトをキューから取得する。
    /// もしキューが空の場合は新しいオブジェクトを生成する。
    /// </summary>
    private GameObject AvailableObject()
    {
        GameObject availableobject = null;

        // キューが空でなく、先頭のオブジェクトが非アクティブな場合
        if (queue.Count > 0 && !queue.Peek().activeSelf)
        {
            //Dequeueはキューの先頭からオブジェクトを取り出すことができるため
            //先頭のオブジェクトが使っている時取り出さない
            availableobject = queue.Dequeue();
        }
        else
        {
            //利用可能なオブジェクトがないから
            //新しいオブジェクトを作って、返す
            availableobject = Copy();
        }
        
        // オブジェクトを再びキューに追加する
        //先頭から取り出したオブジェクトを末に追加する
        //循環させるため
        queue.Enqueue(availableobject);

        return availableobject;
    }
    #region オーバーロード
    /// <summary>
    /// 利用可能なゲームオブジェクトを取得してアクティブ化する
    /// </summary>
    public GameObject preparedObject()
    {
        //オブジェクトを生成する時
        GameObject preparedobject = AvailableObject();
        //アクティブ化する
        preparedobject.SetActive(true);
        return preparedobject;
    }
    
    /// <summary>
    /// 特定の位置を基に生成
    /// </summary>
    /// <param name="position">特定の位置</param>
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
    /// <param name="position">特定の位置</param>
    /// <param name="rotation">特定の回転</param>
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
    /// <param name="position">特定の位置</param>
    /// <param name="rotation">特定の回転</param>
    /// <param name="localScale">特定の拡大・縮小</param>
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
