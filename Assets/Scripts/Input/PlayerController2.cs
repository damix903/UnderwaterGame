using System;
using UnityEngine;
using UnityEngine.InputSystem;
// using MessagePipe;
// using VContainer;

public class PlayerController2 : MonoBehaviour
{
    private IInputProvider _input;
    private CharacterMovement _movement;
    private StateMachine _stateMachine;
    
    private ProjectileShooter _shooter;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float input = 1.5f;

    private bool moving;
    private bool jumping;
    private void Awake()
    {
        _movement = GetComponent<CharacterMovement>();
        _shooter = GetComponent<ProjectileShooter>();
        _input = new InputReader();
        _input.EnableActions(transform);
        _input.OnAttack += OnAttackStarted;

        var idle = new TestIdle();
        var move = new TestMove();
        var jump = new TestJump();
        _stateMachine = new StateMachine();
        
        _stateMachine.AddTransition(idle, move, new FuncPredicate(() => moving));
        _stateMachine.AddTransition(move, idle, new FuncPredicate(() => !moving));
        _stateMachine.AddAnyTransition(jump, new FuncPredicate(() => jumping));
        _stateMachine.AddAnyTransition(idle, new FuncPredicate(() => !jumping && !moving));
        _stateMachine.SetInitialState(idle);
    }

    private void Update()
    {
        _stateMachine.Update();
        _movement.SetMovementInput(new Vector2(_input.MoveInput.x, 0f));
        //_movement.SetMovementInput(_input.MoveInput);
    }

    private void OnAttackStarted(bool started)
    {
        if (!started) return;
        _movement.AddImpulseForce(-_input.AimDir * speed, true);
        _movement.BlockMovement(.5f, input);
        _shooter.Fire(_input.AimDir);
        GetComponent<PlayerHealth>().TakeDamage(new DamageInfo(gameObject, 5f, new EffectData()));
    }

    private void OnDrawGizmos()
    {
        if (_input == null) return;
        if (Camera.main == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, _input.AimDir * 3f);
    }
}
