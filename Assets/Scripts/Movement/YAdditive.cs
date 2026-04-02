using UnityEngine;

public class YAdditive : IHandleInputStrategy
{
    public void HandleInput(Vector2 input, MovementRuntimeStats stats, bool isGrounded, ref Vector2 frameVelocity)
    {
        Vector2 speed = Vector2.one * stats.movementMaxSpeed;
        Vector2 targetSpeed = input * speed;

        if (Mathf.Abs(input.magnitude) > 0.01f)
        {
            float accel = isGrounded ? stats.groundAccel : stats.airAccel;
            // 現在の速度を目標速度に近づける
            frameVelocity.x = Mathf.MoveTowards(frameVelocity.x, targetSpeed.x, accel * Time.fixedDeltaTime);
            
            if (input.y == 0) return;
            frameVelocity.y += input.y > 0f ? accel * Time.fixedDeltaTime : -accel * Time.fixedDeltaTime;
            float absY = Mathf.Abs(targetSpeed.y);
            frameVelocity.y = input.y > 0f ? Mathf.Clamp(frameVelocity.y, -absY, absY) 
                : frameVelocity.y;
        }
        // インプットがないときの減速処理
        else
        {
            float decel = isGrounded ? stats.groundDecel : stats.airDecel;
            frameVelocity = Vector2.MoveTowards(frameVelocity, Vector2.zero, decel * Time.fixedDeltaTime);
        }
    }
}