using UnityEngine;

public interface IMoveable
{
    public void Move(Vector2 position = default);
    public void Stop();
}

public abstract class BaseMove : IMoveable
{
    protected readonly CharacterMovement movement;

    public BaseMove(CharacterMovement movement)
    {
        this.movement = movement;
    }
    
    public abstract void Move(Vector2 position = default);
    public void Stop() => movement.SetMovementInput(Vector2.zero);
}