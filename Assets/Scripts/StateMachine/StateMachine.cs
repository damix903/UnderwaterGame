using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private StateNode _current;
    private readonly Dictionary<Type, StateNode> _nodes = new();
    private readonly HashSet<Transition> _anyTransitions = new();

    public void Update()
    {
        var transition = GetTransition();
        if (transition != null) ChangeState(transition.To);
        
        _current.State?.Update();
    }

    public void FixedUpdate()
    {
        _current.State?.FixedUpdate();
    }

    public void SetInitialState(IState state)
    {
        _current = _nodes[state.GetType()];
        _current.State?.OnEnter();
    }

    private void ChangeState(IState state)
    {
        if (state == _current.State) return;
        
        var next = _nodes[state.GetType()];

        _current.State?.OnExit();
        next.State?.OnEnter();
        _current = next;
    }

    private Transition GetTransition()
    {
        foreach (var t in _anyTransitions)
            if (t.Condition.Evaluate()) return t;
        
        foreach (var t in _current.Transitions)
            if (t.Condition.Evaluate()) return t;
        
        return null;
    }

    public void AddTransition(IState from, IState to, IPredicate condition)
    {
        GetOrAddNode(from).Add(to, condition);
    }
    
    public void AddAnyTransition(IState to, IPredicate condition)
    {
        _anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition));
    }

    private StateNode GetOrAddNode(IState state)
    {
        var node = _nodes.GetValueOrDefault(state.GetType());

        if (node == null)
        {
            node = new StateNode(state);
            _nodes.Add(state.GetType(), node);
        }

        return node;
    }
    
    private class StateNode
    {
        public IState State { get; }
        public HashSet<Transition> Transitions { get; }
        
        public StateNode(IState state)
        {
            State = state;
            Transitions = new HashSet<Transition>();
        }

        public void Add(IState to, IPredicate condition)
        {
            Transitions.Add(new Transition(to, condition));
        }
    }
}