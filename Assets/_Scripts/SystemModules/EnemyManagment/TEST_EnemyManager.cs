using Event;
using EnemyManagment;
using UnityEngine;

public class TEST_EnemyManager : MonoBehaviour
{
    private void Awake()
    {
        WaveManager.Instance.Initialize();
    }

    private void Start()
    {
        WaveManager.Instance.StartWave();
    }
}
