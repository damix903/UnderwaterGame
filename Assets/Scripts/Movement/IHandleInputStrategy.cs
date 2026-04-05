using UnityEngine;

public interface IHandleInputStrategy
{
    void HandleInput(Vector2 input, MovementRuntimeStats stats, bool isGrounded, ref Vector2 frameVelocity);
}