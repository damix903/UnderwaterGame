using System.Threading;
using UnityEngine;
using Attack;

public class AttackState : CharacterBaseState<ICharacterController>
{
    private readonly IAttackable2 _attackable;
    private readonly OverlayAnimData _overlayAnimData;

    private CancellationTokenSource _cts;
    
    public AttackState( ICharacterController owner, IAnimPlayable anim, IAttackable2 attackable, OverlayAnimData overlayAnimData, AnimationClip clip = null) : base(owner, anim, clip)
    {
        _attackable = attackable;
        _overlayAnimData = overlayAnimData;
    }

    public override void OnEnter()
    {
        _cts = new CancellationTokenSource();
        Anim.PlayOverlayAnimation(_overlayAnimData, _cts.Token);
        _attackable?.Attack(owner.Target);
    }
    
    public override void OnExit()
    {
        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }
    }
}