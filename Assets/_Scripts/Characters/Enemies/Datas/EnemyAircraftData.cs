using EnemyManagment;
using UnityEngine;


[CreateAssetMenu(fileName = "Aircraft", menuName = "Data/AircraftType")]
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
    public float MoveRotationAngele;            //  ��]�p�x
}
