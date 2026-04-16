using EnemyAI.Attack;

public class AttackState : CharacterBaseState
{
    protected override AnimType AnimType => AnimType.Attack;
    private readonly IAttackable _attackable;

    public AttackState(IAnimPlayable anim, IAttackable attackable) : base(anim)
    {
        _attackable = attackable;
    }
    
    public override void OnEnter()
    {
        base.OnEnter();
        _attackable?.StartAttack();
    }
    
    public override void OnExit()
    {
        base.OnExit();
        _attackable?.CancelAttack();
    }
}