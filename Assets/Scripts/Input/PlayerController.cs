using System;
using System.Collections;
using ProjectileSystem;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [Inject] private IInputProvider _input;
    private CharacterMovement _movement;
    private StateMachine _stateMachine;

    private IAimable _aimable;
    private IAttackable _attackable;
    
    [SerializeField] private BaseAnimData animData;
    private IAnimPlayable _anim;
    private CharacterMovement2 _movement2;

    private void Awake()
    {
        _movement = GetComponent<CharacterMovement>();

        _aimable = GetComponent<IAimable>();
        _attackable = GetComponent<IAttackable>();
        _input.EnableActions(transform);
        _input.OnAttack += OnAttackStarted;
        _anim = GetComponentInChildren<IAnimPlayable>();
        _movement2 = GetComponent<CharacterMovement2>();
    }

    private void Update()
    {
        //_stateMachine.Update();
        _movement.SetMovementInput(new Vector2(_input.MoveInput.x, 0f));
        _movement2.SetMovementInput(_input.MoveInput);
        
        HandleFlip();
        _aimable.SetAimDirection(_input.AimDir);
        
        if (!_movement.IsGrounded) _anim.PlayBaseClip(animData.FallClip);
        else if (Mathf.Abs(_input.MoveInput.x) > 0.01f) _anim.PlayBaseClip(animData.MoveClip);
        else _anim.PlayBaseClip(animData.IdleClip);
        
    }

    private void HandleFlip()
    {
        if ((_input.AimDir.x < 0f && transform.right.x > 0f)
            || (_input.AimDir.x > 0f && transform.right.x < 0f))
            Flip();
    }

    private void Flip() => transform.Rotate(0f, 180f, 0f);

    private void OnAttackStarted(bool started)
    {
        if (!started) return;
        _attackable.Attack(_input.AimDir);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<ICollectable>(out var collectable))
            collectable.Collect(gameObject);
    }
}
