using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldEnemy : OldCharacter
{
    [SerializeField] int scorePoint = 100;          // �|���ۂɃv���C���[�ɗ^����X�R�A�|�C���g
    [SerializeField] int deathEnergyBonus = 3;      // �|���ۂɃv���C���[�ɗ^����G�l���M�[�{�[�i�X

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        // �|���ۂɃv���C���[�ɗ^����G�l���M�[�{�[�i�X
        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {
            // �v���C���[�����ʃ��\�b�h���Ăяo��
            player.Die();
            // �G�����ʃ��\�b�h���Ăяo��
            Die();
        }
    }

    public override void Die()
    {
        // �X�R�A�}�l�[�W���[�ɃX�R�A�|�C���g��ǉ�����
        ScoreManager.Instance.AddScore(scorePoint);
        // �v���C���[�G�l���M�[�ɃG�l���M�[�{�[�i�X��ǉ�����
        PlayerEnergy.Instance.Obtain(deathEnergyBonus);
        // �G��G�}�l�[�W���[�̃��X�g����폜����
        EnemyManager.Instance.RemoveFromList(gameObject);

        base.Die();
    }
}
