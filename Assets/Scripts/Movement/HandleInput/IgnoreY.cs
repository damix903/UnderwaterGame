using UnityEngine;

public class IgnoreY : IHandleInputStrategy
{
    public void HandleInput(Vector2 input, MovementRuntimeStats stats, bool isGrounded, ref Vector2 frameVelocity)
    {
        Vector2 speed = Vector2.one * stats.movementMaxSpeed;
        Vector2 targetSpeed = input * speed;
        var inputX = Mathf.Abs(frameVelocity.x);
        var targetX = Mathf.Abs(targetSpeed.x);

        if (Mathf.Abs(input.magnitude) > 0.01f && !(inputX * targetX > 0f && inputX > targetX))
        {
            float accel = isGrounded ? stats.groundAccel : stats.airAccel;
            // 現在の速度を目標速度に近づける
            frameVelocity.x = Mathf.MoveTowards(frameVelocity.x, targetSpeed.x, accel * Time.fixedDeltaTime);
            Debug.Log("Accel");
        }
        // インプットがないときの減速処理
        else
        {
            float decel = isGrounded ? stats.groundDecel : stats.airDecel;
            frameVelocity.x = Mathf.MoveTowards(frameVelocity.x, 0f, decel * Time.fixedDeltaTime);
            Debug.Log("Decel");
        }
    }
}