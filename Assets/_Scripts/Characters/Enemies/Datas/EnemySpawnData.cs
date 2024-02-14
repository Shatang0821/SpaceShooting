using Assets.Scripts.Interface;
using EnemyManagment;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawn", menuName = "Data/SpawnType")]
public class EnemySpawnData : ScriptableObject
{
    public EnemyData EnemyData;
    public int EnemyNumber;
    public Vector3 EnemySpawnPos;
    public float SpawnInterval;
}
