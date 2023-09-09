using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // [SerializeField] はUnityエディタから直接値を設定できるようにするための属性。この場合、プールの配列を設定できる。
    [SerializeField] Pool[] playerProjectilePools; //プールの配列を設定できる。

    [SerializeField] Pool[] enemyProjectilePools;

    [SerializeField] Pool[] vFXPools;

    static Dictionary<GameObject, Pool> dictionary; // 各プレハブとそれに関連するオブジェクトプールを関連付けるための辞書。

    void Start()
    {
        dictionary = new Dictionary<GameObject, Pool>();
        
        Initialize(playerProjectilePools);
        Initialize(enemyProjectilePools);
        Initialize(vFXPools);
    }

    // UNITY_EDITORディレクティブは、Unityエディタ環境内でのみコードを実行するためのもの。
    // 実際のゲームプレイでは実行されない。
#if UNITY_EDITOR
    void OnDestroy()
    {
        //プールサイズが正しいかをチェックする
        CheckPoolSize(playerProjectilePools);
        CheckPoolSize(enemyProjectilePools);
        CheckPoolSize(vFXPools);
    }
#endif

    // 各プールのランタイム時のサイズを確認し、初期設定よりも大きい場合は警告を表示する。
    void CheckPoolSize(Pool[] pools)
    {
        foreach (var pool in pools)
        {
            if (pool.RuntimeSize > pool.Size)
            {
                Debug.LogWarning(
                    string.Format("Pool:{0}has a runtime size {1} bigger than its initial size{2}!",
                    pool.Prefab.name,
                    pool.RuntimeSize,
                    pool.Size));
            }
        }
    }

    // プールを初期化し、それぞれのプールを辞書に追加する。
    void Initialize(Pool[] pools)
    {
        foreach (var pool in pools)
        {
#if UNITY_EDITOR    
            if (dictionary.ContainsKey(pool.Prefab))
            {
                Debug.LogError("Same prefab in multiple pools! prefab:" + pool.Prefab.name);
                continue;
            }
#endif
            dictionary.Add(pool.Prefab, pool);

            // Hierarchyビューで見やすくするために新しいGameObjectを作成して、その子としてプールオブジェクトを持つ。
            Transform poolParent = new GameObject("Pool:" + pool.Prefab.name).transform;
            poolParent.parent = transform;

            pool.Initialize(poolParent);
        }
    }


    // 以下のRelease関数群は、指定されたプレハブに基づいてプールからオブジェクトを取得するためのオーバーロードされた関数。
    // もしプールが存在しない、またはプールが空の場合、新しいオブジェクトが作成される。

    /// <summary>
    /// <para>プール内に指定された<paramref name="prefab"></paramref>をゲームオブジェクトに返す。</para>
    /// </summary>
    /// <param name="prefab">
    /// <para>指定されたプレハブ</para>
    /// </param>
    /// <returns>
    /// <para>プール内に準備できたゲームオブジェクト</para>
    /// </returns>
    public static GameObject Release(GameObject prefab)
    {
        #if UNITY_EDITOR
        if(dictionary.ContainsKey(prefab))
        {
            Debug.LogError("pool Manager could NOT find prefab : " + prefab.name);

            return null;
        }
        #endif
        return dictionary[prefab].preparedObject();
    }

    /// <summary>
    /// <para>プール内に指定された<paramref name="prefab"></paramref>をゲームオブジェクトに返す。</para>
    /// </summary>
    /// <param name="prefab">
    /// <para>指定されたプレハブ</para>
    /// </param>
    /// <param name="position">
    /// <para>指定された生成位置</para>
    /// </param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab,Vector3 position)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("pool Manager could NOT find prefab : " + prefab.name);

            return null;
        }
#endif
        return dictionary[prefab].preparedObject(position);
    }

    /// <summary>
    /// <para>プール内に指定された<paramref name="prefab"></paramref>をゲームオブジェクトに返す。</para>
    /// </summary>
    /// <param name="prefab">
    /// <para>指定されたプレハブ</para>
    /// </param>
    /// <param name="position">
    /// <para>指定された生成位置</para>
    /// </param>
    /// <param name="rotation">
    /// <para>指定された回転</para>
    /// </param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position,Quaternion rotation)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("pool Manager could NOT find prefab : " + prefab.name);

            return null;
        }
#endif
        return dictionary[prefab].preparedObject(position,rotation);
    }

    /// <summary>
    /// <para>プール内に指定された<paramref name="prefab"></paramref>をゲームオブジェクトに返す。</para>
    /// </summary>
    /// <param name="prefab">
    /// <para>指定されたプレハブ</para>
    /// </param>
    /// <param name="position">
    /// <para>指定された生成位置</para>
    /// </param>
    /// <param name="rotation">
    /// <para>指定された回転</para>
    /// </param>
    /// <param name="localScale">
    /// <para>指定された拡大・縮小</para>
    /// </param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation,Vector3 localScale)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("pool Manager could NOT find prefab : " + prefab.name);

            return null;
        }
#endif
        return dictionary[prefab].preparedObject(position, rotation,localScale);
    }
}
