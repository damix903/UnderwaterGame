using Attack;
using UnityEngine;

[CreateAssetMenu(fileName = "ESB_", menuName = "Data/State/BasicEnemy")]
public class BasicEnemyStateBuilder : BaseEnemyStateBuilder
{
    [SerializeField] private MoveStrategy moveStrategy;
    [SerializeField] private Vector2 moveDirection;
    [SerializeField] private bool canChase;
    
    public override StateMachine Build(ICharacterController controller, EnemyContext ctx)
    {
        var idle = new IdleState(ctx.Anim, ctx.Data.AnimData.IdleClip);
        var move = new MoveState(ctx.Anim, ctx.Data.AnimData.MoveClip, ResolveMoveable(ctx.Movement));
        
        var stateMachine = new StateMachine();
        stateMachine.AddTransition(idle, move, new FuncPredicate(() => true));
        stateMachine.AddTransition(move, idle, new FuncPredicate(() => false));

        if (canChase)
        {
            var chase = new ChaseState(controller, ctx.Anim, ctx.Data.AnimData.MoveClip, new Chase(ctx.Movement, controller.GameObject.transform));
            stateMachine.AddAnyTransition(chase, new FuncPredicate(() => controller.Target != null));
            stateMachine.AddTransition(chase, idle, new FuncPredicate(() => controller.Target == null));
            
            var attackData = ctx.Data.AttackData;
            if (attackData != null)
            {
                var attackable = ResolveAttackable(controller.GameObject.transform, attackData, ctx.EventListenable);
                var attack = new AttackState(controller, ctx.Anim, attackable, attackData.AnimData);
                stateMachine.AddTransition(chase, attack, new FuncPredicate(()=> controller.Target != null && attackable.CanAttack(controller.Target)));
                stateMachine.AddTransition(attack, idle, new FuncPredicate(() => attackable.IsCompleted));
            }
        }

        
        
        stateMachine.SetInitialState(idle);

        return stateMachine;
    }

    private IMoveable ResolveMoveable(CharacterMovement movement)
    {
        var dir = moveDirection == Vector2.zero
            ? new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f))
            : moveDirection;
        
        return moveStrategy switch
        {
            MoveStrategy.Straight => new StraightMove(movement, dir),
            MoveStrategy.Flip => new BackAndForthMove(movement, dir),
            _ => null
        };
    }
    
    private enum MoveStrategy {Straight, Flip }

    private IAttackable2 ResolveAttackable(Transform owner, EnemyBaseAttackData data, IAnimEventListenable listenable)
    {
        return data.Type switch
        {
            AttackType.Melee => new MeleeAttack(owner, data, listenable),
            //AttackType.Ranged => new RangedAttack(data),
            _ => null
        };
    }
}