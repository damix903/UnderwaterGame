using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

// using MessagePipe;
// using VContainer;

public class PlayerController : MonoBehaviour
{
    [Inject] private IInputProvider _input;
    private CharacterMovement _movement;
    private StateMachine _stateMachine;

    private IAimable _aimable;
    private IAttackable _attackable;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float input = 1.5f;
    
    [SerializeField] private BaseAnimData animData;
    private IAnimPlayable _anim;

    private bool moving;
    private bool jumping;
    private void Awake()
    {
        _movement = GetComponent<CharacterMovement>();

        _aimable = GetComponent<IAimable>();
        _attackable = GetComponent<IAttackable>();
        //_input = new InputReader();
        _input.EnableActions(transform);
        _input.OnAttack += OnAttackStarted;
        _anim = GetComponentInChildren<IAnimPlayable>();
    }

    private void Update()
    {
        //_stateMachine.Update();
        _movement.SetMovementInput(new Vector2(_input.MoveInput.x, 0f));
        
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
}
