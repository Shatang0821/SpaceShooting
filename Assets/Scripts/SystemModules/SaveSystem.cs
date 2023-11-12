using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public static void Save(string saveFileName, object data)
    {
        //date��JSON�f�[�^�ɕς���
        var json = JsonUtility.ToJson(data);
                              //�v���b�g�t�H�[���ɂ���ăp�X��ς���
        var path = Path.Combine(Application.persistentDataPath, saveFileName);

        try
        {
            //json��path��̃t�@�C���ɏ�������
            File.WriteAllText(path, json);

#if UNITY_EDITOR
            Debug.Log($"Successfully saved data to {path}.");
#endif
        }
        catch (System.Exception exception)
        {
#if UNITY_EDITOR
            Debug.LogError($"Failed to saved data to {path}. \n{exception}");
#endif
        }
    }

    public static T Load<T>(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);

        try
        {
            //�f�[�^��ǂݍ���
            var json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<T>(json);

            return data;
        }
        catch (System.Exception exception)
        {
#if UNITY_EDITOR
            Debug.LogError($"Failed to load data from {path}. \n{exception}");
#endif

            return default;
        }
    }

    public static void DeleteSaveFile(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);

        try
        {
            File.Delete(path);
        }
        catch (System.Exception exception)
        {
#if UNITY_EDITOR
            Debug.LogError($"Failed to delete {path}. \n{exception}");
#endif
        }
    }

    /// <summary>
    /// �t�@�C���̑��݃`�F�b�N
    /// </summary>
    /// <param name="saveFileName"></param>
    /// <returns></returns>
    public static bool SaveFileExists(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);

        return File.Exists(path);
    }

}