using System.Collections;
using UnityEngine;

public class AutoDeactivate : MonoBehaviour
{
    //オブジェクトを削除boolen
    [SerializeField] bool destroyGameObject;
    [SerializeField] float lifetime = 3f;

    WaitForSeconds waitLifetime;

    void Awake()
    {
        waitLifetime = new WaitForSeconds(lifetime);
    }

    void OnEnable()
    {
        StartCoroutine(DeactivateCoroutine());
    }

    IEnumerator DeactivateCoroutine()
    {
        yield return waitLifetime;

        if (destroyGameObject)
        {
            Destroy(gameObject);
        }
        else 
        {
            gameObject.SetActive(false);
        }
    }
}