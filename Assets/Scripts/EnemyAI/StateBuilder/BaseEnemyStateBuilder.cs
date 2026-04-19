using StateMachine;
using UnityEngine;

public abstract class BaseEnemyStateBuilder : ScriptableObject
{
    public abstract FiniteStateMachine Build(ICharacterController controller, EnemyContext ctx);
}