using UnityEngine;

public class ChaseState : CharacterBaseState<ICharacterController>
{
    private readonly IMoveable _moveable;


    public ChaseState(ICharacterController owner, IAnimPlayable anim, AnimationClip clip, IMoveable moveable) : base(owner, anim, clip)
    {
        _moveable = moveable;
    }

    public override void FixedUpdate()
    {
        if (owner == null || owner.Target == null) return;
        
        _moveable?.Move(owner.Target.position);
    }
}