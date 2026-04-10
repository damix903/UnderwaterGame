using System.Threading;
using UnityEngine;
using Attack;

public class AttackState : CharacterBaseState<ICharacterController>
{
    private readonly IAttackable2 _attackable;

    public AttackState(ICharacterController owner, IAnimPlayable anim, AnimationClip clip, IAttackable2 attackable) : base(owner, anim, clip)
    {
        _attackable = attackable;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _attackable?.Attack(owner.Target);
    }
}