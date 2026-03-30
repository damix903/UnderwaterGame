using UnityEngine;

[CreateAssetMenu(fileName = "ESB_", menuName = "Data/State/BasicEnemy")]
public class BasicEnemyStateBuilder : BaseEnemyStateBuilder
{
    [SerializeField] private BaseAnimData animData;
    [SerializeField] private MoveStrategy moveStrategy;
    [SerializeField] private Vector2 moveDirection;
    [SerializeField] private bool canChase;
    
    public override StateMachine Build(AIController controller)
    {
        var anim = controller.GetComponentInChildren<AnimationSystem>();

        var movement = controller.GetComponent<CharacterMovement>();
        var idle = new IdleState(anim, animData.IdleClip);
        var move = new MoveState(anim, animData.MoveClip, ResolveMoveable(movement));
        
        var stateMachine = new StateMachine();
        stateMachine.AddTransition(idle, move, new FuncPredicate(() => true));
        stateMachine.AddTransition(move, idle, new FuncPredicate(() => false));

        if (canChase)
        {
            var chase = new ChaseState(anim, animData.MoveClip, new Chase(movement, controller.transform), controller);
            stateMachine.AddAnyTransition(chase, new FuncPredicate(() => controller.Target != null));
            stateMachine.AddTransition(chase, idle, new FuncPredicate(() => controller.Target == null));
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
}