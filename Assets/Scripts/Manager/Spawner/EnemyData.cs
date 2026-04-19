using System;
using EnemyAI;
using EnemyAI.Attack;
using EnemyAI.Move;
using SpawnSystem;
using StateMachine;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Entity/Enemy")]
public class EnemyData : EntityData
{
    [SerializeField] private EnemyType enemyType;
    
    [Header("Behavior Data")]
    [SerializeField] private BaseMoveData baseMoveData;
    [SerializeField] private BaseMoveData chaseMoveData;
    [SerializeField] private BaseMoveData strafeMoveData;
    [SerializeField] private BaseAttackData attackData;
    [SerializeField] private BaseEnemyStateBuilder stateBuilder;
    [SerializeField] private StateType stateType;
    
    [Space]
    [SerializeField] private AnimData animData;
    [SerializeField] private float maxHealth = 1f;

    public EnemyType EnemyType => enemyType;
    public BaseEnemyStateBuilder StateBuilder => stateBuilder;
    public BaseMoveData BaseMoveData => baseMoveData;
    public BaseMoveData ChaseMoveData => chaseMoveData;
    public BaseMoveData StrafeMoveData => strafeMoveData;
    public AnimData AnimData => animData;
    public float MaxHealth => maxHealth;
    public BaseAttackData AttackData => attackData;
    public FiniteStateMachine BuildStateMachine(ICharacterController controller, EnemyContext ctx)
        => EnemyStateRegistry.Build(stateType, controller, ctx);
}

[Serializable]
public enum EnemyType { Ground, Air, Wall}
