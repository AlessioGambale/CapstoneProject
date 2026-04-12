using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Controller<T> : MonoBehaviour where T : Creature
{
    [SerializeField] private FSM_BaseState<T> _initialState;

    private FSM_BaseState<T> _currentState;
    private FSM_BaseState<T>[] _allStates;

    public T Owner { get; private set; }

    private void Awake()
    {
        SetUp();
    }

    private void Start()
    {
        if (_initialState != null)
        {
            SetState(_initialState);
        }
    }

    private void SetUp()
    {
        _allStates = GetComponentsInChildren<FSM_BaseState<T>>();
        Owner = GetComponentInParent<T>();

        foreach (var state in _allStates)
        {
            state.SetUp(this, Owner);
        }
    }

    public void SetState(FSM_BaseState<T> state)
    {
        if (_currentState != null)
        {
            _currentState.OnStateExit();
        }

        _currentState = state;
        _currentState.OnStateEnter();
    }

    private void Update()
    {
        if (_currentState == null) return;

        foreach (var transition in _currentState.Transitions)
        {
            if (transition.IsConditionMet())
            {
                SetState(transition.TargetState);
                break;
            }
        }

        _currentState.StateUpdate();
    }
}
