using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectingState : FSM_BaseState<PlayerCreature>
{
    public override void OnStateEnter()
    {
        // apre UI combat
    }

    public override void StateUpdate() { }

    public override void OnStateExit()
    {
        // chiude UI combat
    }
}