using StateMachine;

namespace EnemyAI
{
    public interface IEnemyStateBuilder
    {
        public StateMachine.FiniteStateMachine Build(ICharacterController controller, EnemyContext ctx);
    }
}