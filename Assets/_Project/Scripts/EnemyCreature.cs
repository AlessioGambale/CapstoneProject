public class EnemyCreature : Creature
{
    public override void Die()
    {

    }
    public override void Hit(float damage)
    {
        base.Hit(damage);
    }

    void Start()
    {
        base.Awake();
        CombatManager.Instance.RegisterEnemy(this);
    }

}

