using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Entity/Enemy")]
public class EnemyData : EntityData
{
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private BaseEnemyStateBuilder stateBuilder;
    [SerializeField] private BaseAnimData animData;

    public EnemyType EnemyType => enemyType;
    public BaseEnemyStateBuilder StateBuilder => stateBuilder;
    public BaseAnimData AnimData => animData;
}

[Serializable]
public enum EnemyType { Ground, Air, Wall}
