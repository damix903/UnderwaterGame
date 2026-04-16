using System;
using EnemyAI.Attack;
using EnemyAI.Move;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Entity/Enemy")]
public class EnemyData : EntityData
{
    [SerializeField] private EnemyType enemyType;
    
    [Header("Behavior Data")]
    [SerializeField] private BaseMoveData baseMoveData;
    [SerializeField] private BaseMoveData chaseMoveData;
    [SerializeField] private BaseAttackData attackData;
    [SerializeField] private BaseEnemyStateBuilder stateBuilder;
    
    [Space]
    [SerializeField] private AnimData animData;
    [SerializeField] private float maxHealth = 1f;

    public EnemyType EnemyType => enemyType;
    public BaseEnemyStateBuilder StateBuilder => stateBuilder;
    public BaseMoveData BaseMoveData => baseMoveData;
    public BaseMoveData ChaseMoveData => chaseMoveData;
    public AnimData AnimData => animData;
    public float MaxHealth => maxHealth;
    public BaseAttackData AttackData => attackData;
}

[Serializable]
public enum EnemyType { Ground, Air, Wall}
