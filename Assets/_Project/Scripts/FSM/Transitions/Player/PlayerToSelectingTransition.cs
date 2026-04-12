using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToSelectingTransition : FSM_BaseTransition<PlayerCreature>
{
    public bool TriggerTransition { get; set; }

    public override bool IsConditionMet() => TriggerTransition;
}
