using System.Collections.Generic;
using StateMachine;

namespace EnemyAI
{
    public enum StateType
    {
        Move,
        Chase,
        AttackWithChase,
        AttackOnly,
        AttackWithStrafe
    }
    
    public static class EnemyStateRegistry
    {
        public static FiniteStateMachine Build(StateType type, ICharacterController controller, EnemyContext ctx)
        {
            return type switch
            {
                StateType.Move => Move(controller, ctx),
                StateType.Chase => Chase(controller, ctx),
                StateType.AttackWithChase => AttackWithChase(controller, ctx),
                StateType.AttackOnly => AttackOnly(controller, ctx),
                StateType.AttackWithStrafe => AttackWithStrafe(controller, ctx),
                _ => null
            };
        }
        
        public static FiniteStateMachine Move(ICharacterController controller, EnemyContext ctx)
        {
            return new EnemyStateBuilder(controller, ctx)
                .WithMove().Build();
        }
        
        public static FiniteStateMachine Chase(ICharacterController controller, EnemyContext ctx)
        {
            return new EnemyStateBuilder(controller, ctx)
                .WithMove().WithChase().Build();
        }
        
        public static FiniteStateMachine AttackWithChase(ICharacterController controller, EnemyContext ctx)
        {
            return new EnemyStateBuilder(controller, ctx)
                .WithMove().WithChase().WithAttack().Build();
        }
        
        public static FiniteStateMachine AttackOnly(ICharacterController controller, EnemyContext ctx)
        {
            return new EnemyStateBuilder(controller, ctx)
                .WithAttack().Build();
        }
        
        public static FiniteStateMachine AttackWithStrafe(ICharacterController controller, EnemyContext ctx)
        {
            return new EnemyStateBuilder(controller, ctx)
                .WithMove().WithChase().WithAttack().WithStrafe().Build();
        }
    }
}