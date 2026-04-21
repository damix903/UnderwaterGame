using System;
using UnityEngine;

namespace Input
{
     public interface IInputProvider
     {
          public Vector2 MoveInput { get; }
          public Vector2 AimDir { get; }
          public event Action<bool> OnAttack;
          public void EnableActions(Transform player);
     }
}