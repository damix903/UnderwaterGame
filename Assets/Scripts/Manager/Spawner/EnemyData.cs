using System;
using Attack;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Entity/Enemy")]
public class EnemyData : EntityData
{
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private BaseEnemyStateBuilder stateBuilder;
    [SerializeField] private AnimData animData;
    [SerializeField] private float maxHealth = 1f;
    [SerializeField] private EnemyBaseAttackData attackData;

    public EnemyType EnemyType => enemyType;
    public BaseEnemyStateBuilder StateBuilder => stateBuilder;
    public AnimData AnimData => animData;
    public float MaxHealth => maxHealth;
    public EnemyBaseAttackData AttackData => attackData;
}

[Serializable]
public enum EnemyType { Ground, Air, Wall}
