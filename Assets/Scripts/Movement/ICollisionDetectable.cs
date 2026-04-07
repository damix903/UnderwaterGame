using System;

public interface ICollisionDetectable
{
    public bool IsGrounded { get; }
    public bool IsWallDetected { get; }
    public event Action OnLanded;
    public event Action OnWallDetected;
}