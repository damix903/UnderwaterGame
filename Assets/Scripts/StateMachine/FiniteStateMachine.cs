using System;
using System.Collections.Generic;
using Utility;

namespace StateMachine
{
    public class FiniteStateMachine
    {
        private StateNode _current;
        private readonly Dictionary<Type, StateNode> _nodes = new();
        private readonly List<Transition> _anyTransitions = new();

        public void Update()
        {
            var transition = GetTransition();
            if (transition != null)
            {
                transition.Condition.Reset();
                ChangeState(transition.To);
            }
        
            _current.State?.Update();
        }

        public void FixedUpdate() => _current.State?.FixedUpdate();

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
    
        // 外部からの呼び出しで強制的に状態を変更するためのメソッド
        public void ChangeState<T>() where T : IState
        {
            if (_nodes.TryGetValue(typeof(T), out var next))
                ChangeState(next.State);
        }

        private Transition GetTransition()
        {
            // Priority順に取り出される
            foreach (var t in _anyTransitions)
                if (t.Condition.Evaluate()) return t;
        
            foreach (var t in _current.Transitions)
                if (t.Condition.Evaluate()) return t;
        
            return null;
        }

        public void AddTransition(IState from, IState to, IPredicate condition, int sortPriority = 0)
        {
            GetOrAddNode(from).Add(to, condition, sortPriority);
        }
    
        public void AddAnyTransition(IState to, IPredicate condition, int sortPriority = 0)
        {
            _anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition, sortPriority));
            _anyTransitions.SortByPriority();
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
    
        /// <summary>
        /// StateとTransitionを保持するクラス
        /// </summary>
        private class StateNode
        {
            public IState State { get; }
            public List<Transition> Transitions { get; } = new List<Transition>();
        
            public StateNode(IState state) => State = state;

            public void Add(IState to, IPredicate condition, int sortPriority = 0)
            {
                Transitions.Add(new Transition(to, condition, sortPriority));
                Transitions.SortByPriority();
            }
        }
    }
}