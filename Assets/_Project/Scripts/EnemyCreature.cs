using UnityEngine;

public class EnemyCreature : Creature
{
    private void Start()
    {
        CombatManager.Instance.RegisterEnemy(this);
    }

    public override void Hit(float damage)
    {
        base.Hit(damage);
    }

    public override void Die()
    {
        Debug.Log("Enemy morto");
    }
}

