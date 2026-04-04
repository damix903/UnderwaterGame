using UnityEngine;

public class YAdditive : IHandleInputStrategy
{
    public void HandleInput(Vector2 input, MovementRuntimeStats stats, bool isGrounded, ref Vector2 inputVelocity)
    {
        Vector2 speed = Vector2.one * stats.movementMaxSpeed;
        Vector2 targetSpeed = input * speed;

        if (Mathf.Abs(input.magnitude) > 0.01f)
        {
            float accel = isGrounded ? stats.groundAccel : stats.airAccel;
            // 現在の速度を目標速度に近づける
            inputVelocity.x = Mathf.MoveTowards(inputVelocity.x, targetSpeed.x, accel * Time.fixedDeltaTime);
            
            if (input.y == 0) return;
            inputVelocity.y += input.y > 0f ? accel * Time.fixedDeltaTime : -accel * Time.fixedDeltaTime;
            float absY = Mathf.Abs(targetSpeed.y);
            inputVelocity.y = input.y > 0f ? Mathf.Clamp(inputVelocity.y, -absY, absY) 
                : inputVelocity.y;
        }
        // インプットがないときの減速処理
        else
        {
            float decel = isGrounded ? stats.groundDecel : stats.airDecel;
            inputVelocity = Vector2.MoveTowards(inputVelocity, Vector2.zero, decel * Time.fixedDeltaTime);
            //frameVelocity.x = Mathf.MoveTowards(frameVelocity.x, 0f, decel * Time.fixedDeltaTime);
        }
    }
}