using UnityEngine;


[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/EnemyData", order = 1)]
public class EnemyAircraftData : ScriptableObject
{
    public AircraftType Type;                   //  AircraftTypeを追加
    public GameObject[] ProjectilePrefabs;      //  敵弾
    public GameObject EnemyPrefab;              //  敵プレハブ
    public float MaxHealth;                     //  最大HP
    public bool ShowOnHeadHealthBar;            //  HPBarの表示
    public int ScorePoint;                      //  倒す際にプレイヤーに与えるスコアポイント
    public int DeathEnergyBonus;                //  倒す際にプレイヤーに与えるエネルギーボーナス
    public float MoveSpeed;                     //  移動速度
    public Vector3 Padding;                     //  敵サイズ
    public float MoveRotationAngele;            //  回転角度
}
