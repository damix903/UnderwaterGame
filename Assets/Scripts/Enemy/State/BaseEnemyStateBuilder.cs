using UnityEngine;

public abstract class BaseEnemyStateBuilder : ScriptableObject
{
    public abstract StateMachine Build(AIController controller, BaseAnimData animData);
}