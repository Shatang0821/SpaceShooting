using EnemyManagment;
using UnityEngine;


[CreateAssetMenu(fileName = "Enemy", menuName = "Data/EnemyType")]
public class EnemyData : ScriptableObject
{
    public GameObject[] ProjectilePrefabs;      //  敵弾
    public GameObject EnemyPrefab;              //  敵プレハブ
    public float MaxHealth;                     //  最大HP
    public bool ShowOnHeadHealthBar;            //  HPBarの表示
    public int ScorePoint;                      //  倒す際にプレイヤーに与えるスコアポイント
    public int DeathEnergyBonus;                //  倒す際にプレイヤーに与えるエネルギーボーナス
    public float MoveSpeed;                     //  移動速度
    public float MoveRotationAngele;            //  回転角度
}
