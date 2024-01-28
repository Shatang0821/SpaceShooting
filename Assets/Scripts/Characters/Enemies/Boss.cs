using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : OldEnemy
{
    BossHealthBar healthBar;

    Canvas healthBarCanvas;

    private void Awake()
    {
        healthBar = FindObjectOfType<BossHealthBar>();
        healthBarCanvas = healthBar.GetComponentInChildren<Canvas>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        healthBar.Initialize(health, maxHealth);
        healthBarCanvas.enabled = true;
    }

    protected override void OnCollisionEnter2D(Collision2D other)
    {
        // �|���ۂɃv���C���[�ɗ^����G�l���M�[�{�[�i�X
        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {
            // �v���C���[�����ʃ��\�b�h���Ăяo��
            player.Die();
        }
    }

    public override void Die()
    {
        healthBarCanvas.enabled = false;
        base.Die();
    }

    public override void TakenDamage(float damage)
    {
        base.TakenDamage(damage);
        healthBar.UpdateStats(health, maxHealth);
    }
}
