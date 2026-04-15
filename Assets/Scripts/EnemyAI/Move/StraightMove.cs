using Movement;
using UnityEngine;

public class StraightMove : BaseMove
{
    private readonly Vector2 _moveDir;


    public StraightMove(CharacterMovement movement, Vector2 moveDir) : base(movement)
    {
        _moveDir = moveDir;
    }

    public override void Move(Vector2 position = default)
    {
        movement?.SetMovementInput(_moveDir);
    }
}