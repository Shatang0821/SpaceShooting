using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // Unity Inspector�Őݒ�\�ȁA
    // �G�A�v���C���[�̃v���W�F�N�^�C���A�G�̃v���W�F�N�^�C���A�G�t�F�N�g�̊e�v�[���z��B
    [SerializeField] ObjectPool[] enemyPools;
    
    [SerializeField] ObjectPool[] playerProjectilePools;

    [SerializeField] ObjectPool[] enemyProjectilePools;

    [SerializeField] ObjectPool[] vFXPools;

    // �v���n�u�Ƃ���ɑΉ�����v�[���̃��t�@�����X���i�[���鎫��
    static Dictionary<GameObject, ObjectPool> dictionary;

    void Awake()
    {
        // �����̏������Ɗe�v�[���̏������B
        dictionary = new Dictionary<GameObject, ObjectPool>();

        Initialize(enemyPools);
        Initialize(playerProjectilePools);
        Initialize(enemyProjectilePools);
        Initialize(vFXPools);
    }

    // Unity�G�f�B�^�ł̂ݎ��s�����f�X�g���N�^�B�e�v�[���̃T�C�Y�����؁B
    // ���ۂ̃Q�[���v���C�ł͎��s����Ȃ��B
#if UNITY_EDITOR
    void OnDestroy()
    {
        //�v�[���T�C�Y�������������`�F�b�N����
        CheckPoolSize(enemyPools);
        CheckPoolSize(playerProjectilePools);
        CheckPoolSize(enemyProjectilePools);
        CheckPoolSize(vFXPools);
    }
#endif

    // �e�v�[�����w�肳�ꂽ�T�C�Y�𒴂��Ă��Ȃ������m�F���A���߂��Ă���ꍇ�͌x����\��
    void CheckPoolSize(ObjectPool[] pools)
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
    /// �v�[�������������A���ꂼ��̃v�[���������ɒǉ�����B
    /// </summary>
    /// <param name="pools">�v���n�u�̔z��</param>
    void Initialize(ObjectPool[] pools)
    {
        //�����v�[���ɈقȂ���̂������Ă��邽�߁A���ꂼ������o��
        foreach (var pool in pools)
        {
#if UNITY_EDITOR    
            //�������̂�����ꍇ�G���[���\������
            if (dictionary.ContainsKey(pool.Prefab))
            {
                //�v���n�u�������v�[��������ꍇ�G���[��\��������
                Debug.LogError("Same prefab in multiple pools! prefab:" + pool.Prefab.name);
                continue;
            }
#endif
            //��Ő�������Ƃ킩��₷��
            //�Ⴆ�΁AEnemy Pools��Enemy01,02,03������
            //01���L�[�Ƃ��Ă��̑Ή��̃v�[�����w��
            dictionary.Add(pool.Prefab, pool);

            // �v�[����Hierarchy�r���[�Ō��₷�����邽�߂ɐV����GameObject���쐬���āA���̎q�Ƃ��ăv�[���I�u�W�F�N�g�����B
            Transform poolParent = new GameObject("Pool:" + pool.Prefab.name).transform;
            poolParent.parent = transform;

            pool.Initialize(poolParent);
        }
    }


    // �ȉ���Release�֐��Q�́A�w�肳�ꂽ�v���n�u�Ɋ�Â��ăv�[������I�u�W�F�N�g���擾���邽�߂̃I�[�o�[���[�h���ꂽ�֐��B
    // �����v�[�������݂��Ȃ��A�܂��̓v�[������̏ꍇ�A�V�����I�u�W�F�N�g���쐬�����B

    /// <summary>
    /// <para>�v�[�����Ɏw�肳�ꂽ<paramref name="prefab"></paramref>���Q�[���I�u�W�F�N�g�ɕԂ��B</para>
    /// </summary>
    /// <param name="prefab">
    /// <para>�w�肳�ꂽ�v���n�u</para>
    /// </param>
    /// <returns>
    /// <para>�v�[�����ɏ����ł����Q�[���I�u�W�F�N�g</para>
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
    /// <para>�v�[�����Ɏw�肳�ꂽ<paramref name="prefab"></paramref>���Q�[���I�u�W�F�N�g�ɕԂ��B</para>
    /// </summary>
    /// <param name="prefab">
    /// <para>�w�肳�ꂽ�v���n�u</para>
    /// </param>
    /// <param name="position">
    /// <para>�w�肳�ꂽ�����ʒu</para>
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
    /// <para>�v�[�����Ɏw�肳�ꂽ<paramref name="prefab"></paramref>���Q�[���I�u�W�F�N�g�ɕԂ��B</para>
    /// </summary>
    /// <param name="prefab">
    /// <para>�w�肳�ꂽ�v���n�u</para>
    /// </param>
    /// <param name="position">
    /// <para>�w�肳�ꂽ�����ʒu</para>
    /// </param>
    /// <param name="rotation">
    /// <para>�w�肳�ꂽ��]</para>
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
    /// <para>�v�[�����Ɏw�肳�ꂽ<paramref name="prefab"></paramref>���Q�[���I�u�W�F�N�g�ɕԂ��B</para>
    /// </summary>
    /// <param name="prefab">
    /// <para>�w�肳�ꂽ�v���n�u</para>
    /// </param>
    /// <param name="position">
    /// <para>�w�肳�ꂽ�����ʒu</para>
    /// </param>
    /// <param name="rotation">
    /// <para>�w�肳�ꂽ��]</para>
    /// </param>
    /// <param name="localScale">
    /// <para>�w�肳�ꂽ�g��E�k��</para>
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
