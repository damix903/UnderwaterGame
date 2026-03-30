using UnityEngine;

public class MoveState : CharacterBaseState
{
    private readonly IMoveable _moveable;

    public MoveState(IAnimPlayable anim, AnimationClip clip, IMoveable moveable) : base(anim, clip)
    {
        _moveable = moveable;
    }

    public override void FixedUpdate()
    {
        _moveable?.Move();
    }
}