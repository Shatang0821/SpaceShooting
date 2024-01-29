using UnityEngine;


[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/EnemyData", order = 1)]
public class EnemyAircraftData : ScriptableObject
{
    public AircraftType Type;                   //  AircraftType��ǉ�
    public GameObject[] ProjectilePrefabs;      //  �G�e
    public GameObject EnemyPrefab;              //  �G�v���n�u
    public float MaxHealth;                     //  �ő�HP
    public bool ShowOnHeadHealthBar;            //  HPBar�̕\��
    public int ScorePoint;                      //  �|���ۂɃv���C���[�ɗ^����X�R�A�|�C���g
    public int DeathEnergyBonus;                //  �|���ۂɃv���C���[�ɗ^����G�l���M�[�{�[�i�X
    public float MoveSpeed;                     //  �ړ����x
    public Vector3 Padding;                     //  �G�T�C�Y
    public float MoveRotationAngele;            //  ��]�p�x
}
