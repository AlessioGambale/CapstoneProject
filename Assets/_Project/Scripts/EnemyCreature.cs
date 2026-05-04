using UnityEngine;

public class EnemyCreature : Creature
{
    private void OnMouseDown()
    {
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
