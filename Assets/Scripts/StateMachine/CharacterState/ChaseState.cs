using UnityEngine;

public class ChaseState : CharacterBaseState<ICharacterController>
{
    protected override AnimType AnimType => AnimType.Move;
    private readonly IMoveable _moveable;


    public ChaseState(ICharacterController owner, IAnimPlayable anim, IMoveable moveable) : base(owner, anim)
    {
        _moveable = moveable;
    }


    public override void FixedUpdate()
    {
        if (owner == null || owner.Target == null) return;
        
        _moveable?.Move(owner.Target.position);
    }

    public override void OnExit()
    {
        base.OnExit();
        _moveable?.Stop();
    }
}