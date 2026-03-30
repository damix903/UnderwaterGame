using System;
using UnityEngine;
using UnityEngine.InputSystem;
// using MessagePipe;
// using VContainer;

public class PlayerController : MonoBehaviour, IInputProvider
{
    private PlayerInputActions _input;
    private CharacterMovement _movement;
    private StateMachine _stateMachine;
    
    private Vector2 _mousePos;
    private Vector2 _dir;
    private ProjectileShooter _shooter;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float input = 1.5f;
    
    private void Awake()
    {
        _movement = GetComponent<CharacterMovement>();
        _input = new PlayerInputActions();
        _shooter = GetComponent<ProjectileShooter>();
    }

    private void Update()
    {
        _mousePos = Mouse.current.position.ReadValue();
        //_stateMachine.Update();
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.Player.Move.performed += OnMovePerformed;
        _input.Player.Move.canceled += OnMoveCanceled;
        _input.Player.Jump.started += OnJumpStarted;
        _input.Player.Jump.canceled += OnJumpEnded;
        _input.Player.Attack.started += OnAttackStarted;
        _input.Player.Look.performed += context =>
        {
            var dir = context.ReadValue<Vector2>();
            _dir = dir;
            Debug.Log($"Looking at {dir}");
        };
    }

    private void OnDisable()
    {
        _input.Disable();
        _input.Player.Move.performed -= OnMovePerformed;
        _input.Player.Move.canceled -= OnMoveCanceled;
        _input.Player.Jump.started -= OnJumpStarted;
        _input.Player.Jump.canceled -= OnJumpEnded;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        _movement.SetMovementInput(moveInput);
        //_resolver.SetDirectionalInput(moveInput);
        MoveInput = moveInput;
    }
    
    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _movement.SetMovementInput(Vector2.zero);
        //_resolver.CancelDirectionalInput();
        MoveInput = Vector2.zero;
    }

    private void OnJumpStarted(InputAction.CallbackContext context)
    {
    }
    
    private void OnJumpEnded(InputAction.CallbackContext context)
    {
    }

    private void OnAttackStarted(InputAction.CallbackContext context)
    {
        //_resolver.AddInput(ActionCommand.LightAttack);
        var pos = Camera.main.ScreenToWorldPoint(_mousePos);
        Vector2 dir = (Vector2)pos - (Vector2)transform.position;
        dir.Normalize();
        _movement.AddImpulseForce(-dir * speed, true);
        _movement.BlockMovement(.5f, input);
        _shooter.Fire(dir);
        GetComponent<PlayerHealth>().TakeDamage(new DamageInfo(gameObject, 5f, new EffectData()));
        OnAttack?.Invoke(true);
    }
    
    private void OnMousePosChanged(InputAction.CallbackContext ctx)
    {
        _mousePos = ctx.ReadValue<Vector2>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (Camera.main == null) return;
        _mousePos = Mouse.current.position.ReadValue();
        var pos = Camera.main.ScreenToWorldPoint(_mousePos);
        //Gizmos.DrawLine(transform.position, pos);
        Gizmos.DrawRay(transform.position, _dir * 3f);
    }

    public Vector2 MoveInput { get; private set; }
    public Vector2 AimDir { get; private set; }
    public event Action<bool> OnAttack;
    public void EnableActions(Transform player)
    {
        
    }
}
