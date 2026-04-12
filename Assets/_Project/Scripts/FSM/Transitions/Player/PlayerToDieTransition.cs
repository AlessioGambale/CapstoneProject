using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToDieTransition : FSM_BaseTransition<PlayerCreature>
{
    public override bool IsConditionMet()
    {
        return _owner.LifeController.IsDead;
    }
}
