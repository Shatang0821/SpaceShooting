using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDeactivate : MonoBehaviour
{

    [SerializeField] bool destroyGameObject;

    [SerializeField] float lifetime = 3f;

    WaitForSeconds waitLifetime;

    private void Awake()
    {
        waitLifetime = new WaitForSeconds(lifetime);
    }

    private void OnEnable()
    {
        StartCoroutine(DeactivateCoroutine());
    }

    IEnumerator DeactivateCoroutine()
    {
        yield return waitLifetime;

        //もしtrueだったらオブジェクトをシーンから削除
        if(destroyGameObject)
        {
            Destroy(gameObject);
        }
        //非アクティブ化にする、すなわち、オブジェクトプールのため
        else
        {
            gameObject.SetActive(false);
        }
    }
}
