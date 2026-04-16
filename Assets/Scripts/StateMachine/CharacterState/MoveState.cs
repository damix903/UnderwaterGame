using EnemyAI.Move;
using UnityEngine;

public class MoveState : CharacterBaseState
{
    private readonly IMoveable _moveable;
    protected override AnimType AnimType => AnimType.Move;

    public MoveState(IAnimPlayable anim, IMoveable moveable) : base(anim)
    {
        _moveable = moveable;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _moveable?.Start();
    }

    public override void FixedUpdate()
    {
        _moveable?.MoveTick();
    }

    public override void OnExit()
    {
        base.OnExit();
        _moveable?.Stop();
    }
}