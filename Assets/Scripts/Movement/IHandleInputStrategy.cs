using UnityEngine;

public interface IHandleInputStrategy
{
    void HandleInput(Vector2 input, MovementRuntimeStats stats, bool isGrounded, ref Vector2 inputVelocity);
}

public class MoveToward : IHandleInputStrategy
{
    public void HandleInput(Vector2 input, MovementRuntimeStats stats, bool isGrounded, ref Vector2 inputVelocity)
    {
        Vector2 speed = Vector2.one * stats.movementMaxSpeed;
        Vector2 targetSpeed = input * speed;

        if (Mathf.Abs(input.magnitude) > 0.01f)
        {
            float accel = isGrounded ? stats.groundAccel : stats.airAccel;
            // 現在の速度を目標速度に近づける
            inputVelocity = Vector2.MoveTowards(inputVelocity, targetSpeed, accel * Time.fixedDeltaTime);
        }
        // インプットがないときの減速処理
        else
        {
            float decel = isGrounded ? stats.groundDecel : stats.airDecel;
            inputVelocity = Vector2.MoveTowards(inputVelocity, Vector2.zero, decel * Time.fixedDeltaTime);
        }
    }
}