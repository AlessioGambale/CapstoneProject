using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreature : Creature
{
    public override void Die()
    {

    }
    public override void Hit(float damage)
    {
            base.Hit(damage);
    }
}
