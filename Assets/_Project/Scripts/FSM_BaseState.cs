using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSM_BaseState<T> : MonoBehaviour where T : Creature
{
    protected FSM_Controller<T> _controller;
    protected FSM_BaseTransition<T>[] _transitions;
    protected T _owner;

    public FSM_BaseTransition<T>[] Transitions => _transitions;

    public virtual void SetUp(FSM_Controller<T> controller, T owner)
    {
        _controller = controller;
        _owner = owner;
        _transitions = GetComponents<FSM_BaseTransition<T>>();

        foreach (var transition in _transitions)
        {
            transition.SetUp(this, _controller, _owner);
        }
    }

    public abstract void OnStateEnter();
    public abstract void StateUpdate();
    public abstract void OnStateExit();
}
