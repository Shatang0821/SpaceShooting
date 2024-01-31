using EnemyManagment;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Data/EnemyType")]
public class EnemySpawnData : ScriptableObject
{
    public EnemyType EnemyType;
    public int EnemuNumber;
    public Vector3 EnemySpawnPos;
    public float SpawnInterval;
}
