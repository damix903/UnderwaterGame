using System.Collections.Generic;
using EnemyAI.Attack;
using StateMachine;
using StateMachine.CharacterState;
using UnityEngine;

namespace EnemyAI
{
    public class EnemyStateBuilder
    {
        private readonly ICharacterController _controller;
        private readonly EnemyContext _ctx;

        private readonly FiniteStateMachine _fsm = new();
        private readonly States _states = new();
        
        private IAttackable _attackable;
        
        public EnemyStateBuilder(ICharacterController controller, EnemyContext ctx)
        {
            _controller = controller;
            _ctx = ctx;
            _states.Idle = new IdleState(_ctx.Anim);
        }

        public EnemyStateBuilder WithMove()
        {
            var moveable = _ctx.Data.BaseMoveData.CreateMove(_ctx.Movement, _controller.GameObject.transform);
            _states.Move = new MoveState(_ctx.Anim, moveable);
            _fsm.AddTransition(_states.Idle, _states.Move, new FuncPredicate(() => true));
            _fsm.AddTransition(_states.Move, _states.Chase, new FuncPredicate(() => false));
            
            return this;
        }
        
        public EnemyStateBuilder WithChase()
        {
            var moveable = _ctx.Data.ChaseMoveData.CreateMove(_ctx.Movement, _controller.GameObject.transform);
            _states.Chase = new ChaseState(_controller, _ctx.Anim, moveable);
            var predicate = new FuncPredicate(() => _controller.Target != null);
            
            _fsm.AddTransition(_states.Idle, _states.Chase, predicate);
            if (_states.Move != null) _fsm.AddTransition(_states.Move, _states.Chase, predicate);
            _fsm.AddTransition(_states.Chase, _states.Idle, new FuncPredicate(() => _controller.Target == null));

            return this;
        }

        public EnemyStateBuilder WithAttack()
        {
            _attackable = _ctx.Data.AttackData.CreateAttack(_controller, _ctx.EventListenable);
            _states.Attack = new AttackState(_ctx.Anim, _attackable);

            var predicate = new FuncPredicate(() => _attackable.CanAttack);
            _fsm.AddTransition(_states.Idle, _states.Attack, predicate, 10);
            _fsm.AddAnyTransition(_states.Attack, predicate, 5);
            _fsm.AddTransition(_states.Attack, _states.Idle, new FuncPredicate(() => _attackable.IsCompleted), 5);
            
            return this;
        }
        
        public EnemyStateBuilder WithStrafe()
        {
            var moveable = _ctx.Data.StrafeMoveData.CreateMove(_ctx.Movement, _controller.GameObject.transform);
            _states.Strafe = new StrafeState(_controller, _ctx.Anim, moveable);
            
            if (_attackable != null)
            {
                _fsm.AddTransition(_states.Attack, _states.Strafe, new FuncPredicate(() => _attackable.IsCompleted));
                _fsm.AddAnyTransition(_states.Strafe,
                    new FuncPredicate(() => _controller.Target != null && _attackable.CanAttack), 10);
            }
            else Debug.LogWarning($"Strafe state needs attack state {_ctx.Data.name}");
            
            _fsm.AddTransition(_states.Strafe, _states.Idle, new FuncPredicate(() => _controller.Target == null));

            return this;
        }

        public FiniteStateMachine Build()
        {
            _fsm.SetInitialState(_states.Idle);
            return _fsm;
        }

        private class States
        {
            public IState Idle { get; set; }
            public IState Move { get; set; }
            public IState Chase { get; set; }
            public IState Attack { get; set; }
            public IState Strafe { get; set; }
            public IState Death { get; set; }
        }
    }
}