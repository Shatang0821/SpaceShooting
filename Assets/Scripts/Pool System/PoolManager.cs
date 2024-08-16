using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // Unity Inspectorで設定可能な、
    // 敵、プレイヤーのプロジェクタイル、敵のプロジェクタイル、エフェクトの各プール配列。
    [SerializeField] Pool[] enemyPools;
    
    [SerializeField] Pool[] playerProjectilePools;

    [SerializeField] Pool[] enemyProjectilePools;

    [SerializeField] Pool[] vFXPools;

    // プレハブとそれに対応するプールのリファレンスを格納する辞書
    static Dictionary<GameObject, Pool> dictionary;

    void Awake()
    {
        // 辞書の初期化と各プールの初期化。
        dictionary = new Dictionary<GameObject, Pool>();

        Initialize(enemyPools);
        Initialize(playerProjectilePools);
        Initialize(enemyProjectilePools);
        Initialize(vFXPools);
    }

    // Unityエディタでのみ実行されるデストラクタ。各プールのサイズを検証。
    // 実際のゲームプレイでは実行されない。
#if UNITY_EDITOR
    void OnDestroy()
    {
        //プールサイズが正しいかをチェックする
        CheckPoolSize(enemyPools);
        CheckPoolSize(playerProjectilePools);
        CheckPoolSize(enemyProjectilePools);
        CheckPoolSize(vFXPools);
    }
#endif

    // 各プールが指定されたサイズを超えていないかを確認し、超過している場合は警告を表示
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

    /// <summary>
    /// プールを初期化し、それぞれのプールを辞書に追加する。
    /// </summary>
    /// <param name="pools">プレハブの配列</param>
    void Initialize(Pool[] pools)
    {
        //同じプールに異なるものが入っているため、それぞれを取り出す
        foreach (var pool in pools)
        {
#if UNITY_EDITOR    
            //同じものがある場合エラーが表示する
            if (dictionary.ContainsKey(pool.Prefab))
            {
                //プレハブが同じプールがある場合エラーを表示させる
                Debug.LogError("Same prefab in multiple pools! prefab:" + pool.Prefab.name);
                continue;
            }
#endif
            //例で説明するとわかりやすい
            //例えば、Enemy PoolsにEnemy01,02,03がある
            //01をキーとしてその対応のプールを指す
            dictionary.Add(pool.Prefab, pool);

            // プールをHierarchyビューで見やすくするために新しいGameObjectを作成して、その子としてプールオブジェクトを持つ。
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
        if(!dictionary.ContainsKey(prefab))
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
