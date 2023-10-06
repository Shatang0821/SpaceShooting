using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] int deathEnergyBonus = 3;

    public override void Die()
    {
        PlayerEnergy.Instance.Obtain(deathEnergyBonus);
        //list ‚©‚ç©•ª‚ğíœ
        EnemyManager.Instance.RemoveFromList(gameObject);
        base.Die();
    }
}
