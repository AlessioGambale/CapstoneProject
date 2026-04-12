using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyToHitTransition : FSM_BaseTransition<EnemyCreature>
{
    private int _previousHealth;

    public override void SetUp(FSM_BaseState<EnemyCreature> ownerState, FSM_Controller<EnemyCreature> controller, EnemyCreature owner)
    {
        base.SetUp(ownerState, controller, owner);
        _previousHealth = _owner.LifeController.CurrentHealth;
    }

    public override bool IsConditionMet()
    {
        if (_owner.LifeController.CurrentHealth < _previousHealth)
        {
            _previousHealth = _owner.LifeController.CurrentHealth;
            return true;
        }
        return false;
    }
}
