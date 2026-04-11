using Attack;
using UnityEngine;

public abstract class BaseEnemyStateBuilder : ScriptableObject
{
    public abstract StateMachine Build(ICharacterController controller, EnemyContext ctx);
}