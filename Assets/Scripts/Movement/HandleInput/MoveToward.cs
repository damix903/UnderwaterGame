using UnityEngine;

public class MoveToward : IHandleInputStrategy
{
    public void HandleInput(Vector2 input, MovementRuntimeStats stats, bool isGrounded, ref Vector2 frameVelocity)
    {
        Vector2 speed = Vector2.one * stats.movementMaxSpeed;
        Vector2 targetSpeed = input * speed;

        if (Mathf.Abs(input.magnitude) > 0.01f && Mathf.Abs(frameVelocity.x) <= Mathf.Abs(targetSpeed.x))
        {
            float accel = isGrounded ? stats.groundAccel : stats.airAccel;
            // 現在の速度を目標速度に近づける
            frameVelocity = Vector2.MoveTowards(frameVelocity, targetSpeed, accel * Time.fixedDeltaTime);
        }
        // インプットがないときの減速処理
        else
        {
            float decel = isGrounded ? stats.groundDecel : stats.airDecel;
            frameVelocity = Vector2.MoveTowards(frameVelocity, Vector2.zero, decel * Time.fixedDeltaTime);
        }
    }
}