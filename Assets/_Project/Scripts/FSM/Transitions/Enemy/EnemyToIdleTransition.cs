using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyToIdleTransition : FSM_BaseTransition<EnemyCreature>
{
    public bool TriggerTransition { get; set; }

    public override bool IsConditionMet() => TriggerTransition;
}
