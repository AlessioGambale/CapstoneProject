using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSM_BaseTransition<T> : MonoBehaviour where T : Creature
{
    [SerializeField] private FSM_BaseState<T> _targetState;

    protected FSM_BaseState<T> _ownerState;
    protected FSM_Controller<T> _controller;
    protected T _owner;

    public FSM_BaseState<T> TargetState => _targetState;

    public virtual void SetUp(FSM_BaseState<T> ownerState, FSM_Controller<T> controller, T owner)
    {
        _ownerState = ownerState;
        _controller = controller;
        _owner = owner;
    }

    public abstract bool IsConditionMet();
}
