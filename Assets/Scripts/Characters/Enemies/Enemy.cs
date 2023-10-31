using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] int scorePoint = 100;
    [SerializeField] int deathEnergyBonus = 3;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.Die();
            Die();
        }
    }

    public override void Die()
    {
        ScoreManager.Instance.AddScore(scorePoint);
        PlayerEnergy.Instance.Obtain(deathEnergyBonus);
        //list ‚©‚ç©•ª‚ğíœ
        EnemyManager.Instance.RemoveFromList(gameObject);
        base.Die();
    }
}
