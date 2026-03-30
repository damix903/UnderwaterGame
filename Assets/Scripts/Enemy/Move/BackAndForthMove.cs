using UnityEngine;

public class BackAndForthMove : BaseMove
{
    private Vector2 _moveDir;
    private float _timer;
    
    public BackAndForthMove(CharacterMovement movement, Vector2 dir) : base(movement)
    {
        _moveDir = dir;
        _timer = 2f;
    }

    public override void Move(Vector2 position = default)
    {
        movement.SetMovementInput(_moveDir);
        _timer -= Time.fixedDeltaTime;
        if (_timer <= 0)
        {
            _moveDir *= -1;
            _timer = 2f;
        }
    }
}