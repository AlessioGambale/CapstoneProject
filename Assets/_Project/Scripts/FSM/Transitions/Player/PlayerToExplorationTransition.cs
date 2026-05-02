using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToExplorationTransition : FSM_BaseTransition<PlayerCreature>
{
    private bool _trigger;

    public void Trigger() => _trigger = true;

    public override bool IsConditionMet()
    {
        if (_trigger)
        {
            _trigger = false;
            return true;
        }
        return false;
    }
}