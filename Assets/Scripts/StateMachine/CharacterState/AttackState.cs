using System.Threading;
using UnityEngine;
using Attack;

public class AttackState : CharacterBaseState<ICharacterController>
{
    protected override AnimType AnimType => AnimType.Attack;
    private readonly IAttackable2 _attackable;

    public AttackState(ICharacterController owner, IAnimPlayable anim, IAttackable2 attackable) : base(owner, anim)
    {
        _attackable = attackable;
    }


    public override void OnEnter()
    {
        base.OnEnter();
        _attackable?.Attack(owner.Target);
    }
}