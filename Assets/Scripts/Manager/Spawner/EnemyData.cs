using System;
using EnemyAI;
using EnemyAI.Attack;
using EnemyAI.Move;
using Movement;
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
	[SerializeField] private StateType stateType;
	
	[Space]
	[SerializeField] private AnimData animData;
	[SerializeField] private float maxHealth = 1f;

	public EnemyType EnemyType => enemyType;
	public AnimData AnimData => animData;
	public float MaxHealth => maxHealth;

	public IMoveable BaseMoveable(CharacterMovement movement, Transform owner)
		=> baseMoveData?.CreateMove(movement, owner);

    public IMoveable ChaseMoveable(CharacterMovement movement, Transform owner)
        => chaseMoveData?.CreateMove(movement, owner);

    public IMoveable StrafeMoveable(CharacterMovement movement, Transform owner)
        => strafeMoveData?.CreateMove(movement, owner);

	public IAttackable Attackable(ICharacterController controller, IAnimEventListenable listenable)
		=> attackData.CreateAttack(controller, listenable);

    public FiniteStateMachine BuildStateMachine(ICharacterController controller, EnemyContext ctx)
		=> EnemyStateRegistry.Build(stateType, controller, ctx);
}

[Serializable]
public enum EnemyType { Ground, Air, Wall}
