using UnityEngine;

public class ChaseState : CharacterBaseState
{
    private readonly IMoveable _moveable;
    private readonly AIController _controller;

    public ChaseState(IAnimPlayable anim, AnimationClip clip, IMoveable moveable, AIController controller) : base(anim, clip)
    {
        _moveable = moveable;
        _controller = controller;
    }

    public override void FixedUpdate()
    {
        _moveable?.Move(_controller.Target.transform.position);
    }
}