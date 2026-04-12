using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyToDieTransition : FSM_BaseTransition<EnemyCreature>
{
    public override bool IsConditionMet()
    {
        return _owner.LifeController.IsDead;
    }
}
