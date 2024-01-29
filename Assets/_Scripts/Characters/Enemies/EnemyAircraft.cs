using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.Characters.Enemies
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
