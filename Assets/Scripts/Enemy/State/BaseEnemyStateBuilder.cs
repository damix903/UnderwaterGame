using Attack;
using UnityEngine;

public abstract class BaseEnemyStateBuilder : ScriptableObject
{
    public abstract StateMachine Build(ICharacterController controller, EnemyContext ctx);
}

public struct EnemyContext
{
    public EnemyData Data;
    public CharacterMovement Movement;
    public IAnimPlayable Anim;
    public IAnimEventListenable EventListenable;

    public EnemyContext(EnemyData data, CharacterMovement movement, IAnimPlayable anim, IAnimEventListenable eventListenable)
    {
        Data = data;
        Movement = movement;
        Anim = anim;
        EventListenable = eventListenable;
    }
}