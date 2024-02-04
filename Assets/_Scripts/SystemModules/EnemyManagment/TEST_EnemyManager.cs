using Event;
using EnemyManagment;
using UnityEngine;

public class TEST_EnemyManager : MonoBehaviour
{
    private WaveManager _waveManager;
    private SpawnManager _spawnManager;
    private void Awake()
    {
        //_waveManager = new WaveManager();
        //_spawnManager = new SpawnManager();


    }

    private void Start()
    {
        //_waveManager.Initialize();
    }

    private void Update()
    {
        //switch (_waveManager.CurrentState)
        //{
        //    case WaveState.Waiting:
        //        break;
        //    case WaveState.InProgress:
        //        break;
        //    case WaveState.Completed: 
        //        break;
        //}
    }
}
