using UnityEngine;

public class EnemyCreature : Creature
{
    private void OnMouseDown()
    {
        Debug.Log($"[Enemy] Cliccato: {gameObject.name}");
        CombatManager.Instance.SelectTarget(this);
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

