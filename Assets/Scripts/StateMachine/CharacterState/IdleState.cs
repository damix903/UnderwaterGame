using UnityEngine;

public class IdleState : CharacterBaseState
{
    protected override AnimType AnimType => AnimType.Idle;
    public IdleState(IAnimPlayable anim) : base(anim) { }
}