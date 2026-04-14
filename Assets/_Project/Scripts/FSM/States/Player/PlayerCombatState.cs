using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatState : FSM_BaseState<PlayerCreature>
{
    private PlayerStateHandler _stateHandler;

    public override void SetUp(FSM_Controller<PlayerCreature> controller, PlayerCreature owner)
    {
        base.SetUp(controller, owner);
        _stateHandler = owner.GetComponent<PlayerStateHandler>();
    }

    public override void OnStateEnter()
    {
        _stateHandler.EnterCombat();
    }

    public override void StateUpdate() { }

    public override void OnStateExit()
    {
        _stateHandler.ExitCombat();
    }
}

