using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldEnemy : OldCharacter
{
    [SerializeField] int scorePoint = 100;          // 倒す際にプレイヤーに与えるスコアポイント
    [SerializeField] int deathEnergyBonus = 3;      // 倒す際にプレイヤーに与えるエネルギーボーナス

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        // 倒す際にプレイヤーに与えるエネルギーボーナス
        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {
            // プレイヤーが死ぬメソッドを呼び出す
            player.Die();
            // 敵が死ぬメソッドを呼び出す
            Die();
        }
    }

    public override void Die()
    {
        // スコアマネージャーにスコアポイントを追加する
        ScoreManager.Instance.AddScore(scorePoint);
        // プレイヤーエネルギーにエネルギーボーナスを追加する
        PlayerEnergy.Instance.Obtain(deathEnergyBonus);
        // 敵を敵マネージャーのリストから削除する
        EnemyManager.Instance.RemoveFromList(gameObject);

        base.Die();
    }
}
