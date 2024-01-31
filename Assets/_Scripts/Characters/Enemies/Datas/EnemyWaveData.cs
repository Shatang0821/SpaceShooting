using EnemyManagment;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Data/WaveType")]
public class EnemyWaveData : ScriptableObject
{
    public WaveType WaveType;
    public EnemySpawnData[] EnemySpawnDatas;
}