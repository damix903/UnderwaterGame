using System;
using UnityEngine;

public interface IInputProvider
{
     public Vector2 MoveInput { get; }
     public Vector2 AimDir { get; }
     public event Action<bool> OnAttack;
     public void EnableActions(Transform player);
}