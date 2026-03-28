using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInputActions;

public class InputReader : IInputProvider, IPlayerActions
{
    private PlayerInputActions _input;
    private Transform _player;

    public Vector2 MoveInput { get; private set; }
    public Vector2 AimDir { get; private set; }
    public event Action<bool> OnAttack;
    
    public void EnableActions(Transform player)
    {
        if (_input == null)
        {
            _input = new PlayerInputActions();
            _input.Player.SetCallbacks(this);
        }
        _input.Enable();
        _player = player;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        AimDir = context.ReadValue<Vector2>().normalized;
    }

    public void OnLookMouse(InputAction.CallbackContext context)
    {
        var pos = context.ReadValue<Vector2>();

        if (_player == null) return;
        if (Camera.main == null) return;
        var worldPos = Camera.main.ScreenToWorldPoint(pos);
        AimDir = ((Vector2)worldPos - (Vector2)_player.position).normalized;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        
    }

    void IPlayerActions.OnAttack(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                OnAttack?.Invoke(true);
                break;
            
            case InputActionPhase.Canceled:
                OnAttack?.Invoke(false);
                break;
        }
    }
}
