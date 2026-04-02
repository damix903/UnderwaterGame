using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

// using MessagePipe;
// using VContainer;

public class PlayerController2 : MonoBehaviour
{
    [Inject] private IInputProvider _input;
    private CharacterMovement _movement;
    private StateMachine _stateMachine;
    
    private ProjectileShooter _shooter;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float input = 1.5f;
    
    [SerializeField] private BaseAnimData animData;
    private IAnimPlayable _anim;

    private bool moving;
    private bool jumping;
    private void Awake()
    {
        _movement = GetComponent<CharacterMovement>();
        _shooter = GetComponent<ProjectileShooter>();
        //_input = new InputReader();
        _input.EnableActions(transform);
        _input.OnAttack += OnAttackStarted;
        _anim = GetComponentInChildren<IAnimPlayable>();
    }

    private void Update()
    {
        //_stateMachine.Update();
        _movement.SetMovementInput(new Vector2(_input.MoveInput.x, 0f));
        if ((_input.AimDir.x < 0f && transform.right.x > 0f)
            || (_input.AimDir.x > 0f && transform.right.x < 0f)) Flip();
        
        if (!_movement.IsGrounded) _anim.PlayBaseClip(animData.FallClip);
        else if (Mathf.Abs(_input.MoveInput.x) > 0.01f) _anim.PlayBaseClip(animData.MoveClip);
        else _anim.PlayBaseClip(animData.IdleClip);
    }
    
    private void Flip() => transform.Rotate(0f, 180f, 0f);

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
