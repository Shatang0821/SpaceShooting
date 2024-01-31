using UnityEngine;

namespace EnemyManagment
{
    public class EnemyAircraft
    {
        public GameObject EnemyPrefab {  get; private set; }
        public GameObject[] ProjectilePrefabs {  get; private set; }
        public Transform MuzzleTransform {  get; private set; }
        public const string muzzle = "Muzzle";
        public EnemyAircraft(GameObject enemyPrefab, GameObject[] projectilePrefabs)
        {
            this.EnemyPrefab = enemyPrefab;
            this.ProjectilePrefabs = projectilePrefabs;
            this.MuzzleTransform = enemyPrefab.transform.Find(muzzle);
        }
    }
}
