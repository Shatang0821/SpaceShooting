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

        //����true��������I�u�W�F�N�g���V�[������폜
        if(destroyGameObject)
        {
            Destroy(gameObject);
        }
        //��A�N�e�B�u���ɂ���A���Ȃ킿�A�I�u�W�F�N�g�v�[���̂���
        else
        {
            gameObject.SetActive(false);
        }
    }
}
