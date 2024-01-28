using Assets.Scripts.Characters.Enemies;
using Assets.Scripts.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_EnemyManager : MonoBehaviour
{
    public GameObject _projectile;
    private List<IEnemyBehavior> _enemies = new List<IEnemyBehavior>();
    private void Start()
    {
        //var muzzle = gameobject.transform.Find("Muzzle");
        //IEnemyBehavior enemy = new EnemyBehavior(gameobject, _projectile, muzzle);
        //_enemies.Add(enemy);
        //StartEnemyAttack(enemy);
    }

    private void FixedUpdate()
    {
        foreach (var enemy in _enemies)
        {
            enemy.Move();
        }
    }

    private void StartEnemyAttack(IEnemyBehavior enemy)
    {
        StartCoroutine(enemy.Attack());
    }
}
