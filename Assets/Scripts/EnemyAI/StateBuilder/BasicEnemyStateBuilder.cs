using StateMachine;
using UnityEngine;

[CreateAssetMenu(fileName = "ESB_", menuName = "Data/State/BasicEnemy")]
public class BasicEnemyStateBuilder : BaseEnemyStateBuilder
{
    [SerializeField] private bool canChase;
    
    public override FiniteStateMachine Build(ICharacterController controller, EnemyContext ctx)
    {
        var idle = new IdleState(ctx.Anim);
        var move = new MoveState(ctx.Anim, ctx.Moveable);
        
        var stateMachine = new FiniteStateMachine();
        stateMachine.AddTransition(idle, move, new FuncPredicate(() => true));
        stateMachine.AddTransition(move, idle, new FuncPredicate(() => false));

        if (canChase)
        {
            var chase = new ChaseState(controller, ctx.Anim, ctx.ChaseMoveable);
            var attackData = ctx.Data.AttackData;
            if (attackData != null)
            {
                var attackable = ctx.Data.AttackData.CreateAttack(controller, ctx.EventListenable);
                var attack = new AttackState(ctx.Anim, attackable);
                stateMachine.AddAnyTransition(attack, new FuncPredicate(()=> attackable.CanAttack));
                stateMachine.AddTransition(attack, idle, new FuncPredicate(() => attackable.IsCompleted));
            }
            stateMachine.AddAnyTransition(chase, new FuncPredicate(() => controller.Target != null));
            stateMachine.AddTransition(chase, idle, new FuncPredicate(() => controller.Target == null));
            
        }
        
        stateMachine.SetInitialState(idle);

        return stateMachine;
    }
}