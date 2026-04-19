using StateMachine;

namespace EnemyAI
{
    public class NormalEnemyStateBuilder : IEnemyStateBuilder
    {
        public FiniteStateMachine Build(ICharacterController controller, EnemyContext ctx)
        {
            var moveable = ctx.Data.BaseMoveData.CreateMove(ctx.Movement, controller.GameObject.transform);
            var idle = new IdleState(ctx.Anim);
            var move = new MoveState(ctx.Anim, moveable);
            
            var stateMachine = new FiniteStateMachine();
            stateMachine.AddTransition(idle, move, new FuncPredicate(() => true));
            stateMachine.AddTransition(move, idle, new FuncPredicate(() => false));

            return stateMachine;
        }
    }
}