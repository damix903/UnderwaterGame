using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Manager.AudioSystem;
using Movement;
using ProjectileSystem;
using StateMachine;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [Inject] private IInputProvider _input;
    private CharacterMovement _movement;
    //private StateMachine _stateMachine;

    private IAimable _aimable;
    private IAttackable _attackable;
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private SoundData _soundData;
    [SerializeField] private SoundData _soundData2;
    
    [SerializeField] private AnimData animData;
    private IAnimPlayable _anim;

    private void Awake()
    {
        _movement = GetComponent<CharacterMovement>();

        _aimable = GetComponent<IAimable>();
        _attackable = GetComponent<IAttackable>();
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
        
        if (!_movement.IsGrounded) _anim.PlayBaseClip(animData.GetAnim(AnimType.Fall));
        else if (Mathf.Abs(_input.MoveInput.x) > 0.01f) _anim.PlayBaseClip(animData.GetAnim(AnimType.Move));
        else _anim.PlayBaseClip(animData.GetAnim(AnimType.Idle));
        
        var token = this.GetCancellationTokenOnDestroy();
        var data = _input.MoveInput.x > 0f ? _soundData : _soundData2;
        
        if (Mathf.Abs(_input.MoveInput.x) > 0.01f)
        _soundManager.StartBGM(data,3f, ct: token).Forget();
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
