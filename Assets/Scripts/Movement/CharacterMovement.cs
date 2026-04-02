using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
    private enum MovementMode { None, Normal}
    private MovementMode _movementMode = MovementMode.Normal;

    [SerializeField] private bool shouldFlip = true;
    [SerializeField] private bool isFacingRight = true;
    
    [Header("Collision Check")]
    [SerializeField] private LayerMask groundLayer;
    
    [SerializeField] private MovementStats baseStats;
    private MovementRuntimeStats _currentStats;
    private IMovementModifier _modifier;
    
    private Rigidbody2D _rb;
    private IHandleInputStrategy _strategy;

    #region Private Fields

    private Vector2 _input;

    private float _flipScale;
    private float _movementBlockTimer;
    private bool _shouldBlockY;
    private float _inputMul;
    
    private Vector2 _frameVelocity;
    private Vector2 _impulseVelocity;
    private Vector2 _constantVelocity;
    
    #endregion

    public Vector2 Velocity => _rb.linearVelocity;
    public bool IsGrounded { get; private set; }
    public bool IsWallDetected { get; private set; }
    public bool IsFalling => !IsGrounded && _rb.linearVelocity.y < 0f;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null) _rb = gameObject.AddComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;
        _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        _rb.freezeRotation = true;

        _currentStats = baseStats.Stats;
        _flipScale = isFacingRight ? 180f : -180f;

        _strategy = baseStats.Strategy();
    }

    public void SetMovementInput(Vector2 input) => _input = input;
    public void SetMovementModifier(IMovementModifier modifier) => _modifier = modifier;

    public void AddImpulseForce(Vector2 force, bool overwrite = false)
    {
        _impulseVelocity = overwrite ? force : _impulseVelocity + force;
    }

    public void AddConstantForce(Vector2 force) => _constantVelocity += force * Time.fixedDeltaTime;
    public void RemoveConstantForce(Vector2 force) => _constantVelocity -= force * Time.fixedDeltaTime;

    public void BlockMovement(float duration, float inputMultiplier = 0f, bool blockY = false)
    {
        _movementMode = MovementMode.None;
        _movementBlockTimer = duration;
        _shouldBlockY = blockY;
        _inputMul = inputMultiplier;
    }

    public void EnableMove() => _movementBlockTimer = 0f;
    public void EnableY() => _shouldBlockY = false;

    private void FixedUpdate()
    {
        _currentStats = baseStats.Stats;
        _frameVelocity = _rb.linearVelocity;

        HandleCollisionDetection();
        //_currentStats = baseStats.Stats;
        if (_modifier != null) _currentStats = _modifier.Apply(_currentStats);

        switch (_movementMode)
        {
            case MovementMode.Normal:
                HandleInput();
                HandleFlip();
                break;
            
            case MovementMode.None:
                HandleNoneMove();
                HandleInput();
                break;
        }

        HandleImpulseForce();
        
        var finalVel = _frameVelocity + _constantVelocity;

        float gravity = finalVel.y > 0f ? _currentStats.upwardGravityScale : _currentStats.defaultGravityScale;
        finalVel.y -= gravity * Time.fixedDeltaTime;
        
        finalVel.x = Mathf.Clamp(finalVel.x, -_currentStats.maxSpeed, _currentStats.maxSpeed);
        finalVel.y = Mathf.Clamp(finalVel.y, -_currentStats.maxSpeed, _currentStats.maxSpeed);
        
        _rb.linearVelocity = finalVel;
        //Debug.Log($"{_frameVelocity} : {_rb.linearVelocity}");
    }

    private void HandleNoneMove()
    {
        if (_shouldBlockY)
        {
            _rb.gravityScale = 0f;
            _frameVelocity.y = 0f;
        }
        
        _movementBlockTimer -= Time.fixedDeltaTime;
        if (_movementBlockTimer < 0f)
        {
            _movementMode = MovementMode.Normal;
        }
    }

    private void HandleInput()
    {
        Vector2 effectiveInput = _movementBlockTimer > 0f ? _input * _inputMul : _input;
        _strategy.HandleInput(effectiveInput, _currentStats, IsGrounded, ref _frameVelocity);
        
        // Vector2 speed = Vector2.one * _currentStats.movementMaxSpeed;
        // Vector2 targetSpeed = effectiveInput * speed;

        // if (Mathf.Abs(_input.magnitude) > 0.01f && Mathf.Abs(effectiveInput.magnitude) > 0.01f)
        // {
        //     float accel = IsGrounded ? _currentStats.groundAccel : _currentStats.airAccel;
        //     // 現在の速度を目標速度に近づける
        //     _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, targetSpeed.x, accel * Time.deltaTime);
        //     
        //     if (_input.y == 0) return;
        //     _frameVelocity.y += _input.y > 0f ? accel * Time.fixedDeltaTime : -accel * Time.deltaTime;
        //     float absY = Mathf.Abs(targetSpeed.y);
        //     _frameVelocity.y = _input.y > 0f ? Mathf.Clamp(_frameVelocity.y, -absY, absY) 
        //         : _frameVelocity.y;
        // }
        // // インプットがないときの減速処理
        // else
        // {
        //     float decel = IsGrounded ? _currentStats.groundDecel : _currentStats.airDecel;
        //     _frameVelocity = Vector2.MoveTowards(_frameVelocity, Vector2.zero, decel * Time.fixedDeltaTime);
        // }
    }

    private void HandleImpulseForce()
    {
        if (_impulseVelocity == Vector2.zero) return;

        _frameVelocity += _impulseVelocity;
        _impulseVelocity = Vector2.zero;
    }
    
    private void HandleCollisionDetection()
    {
        IsGrounded = Physics2D.Raycast(
            GetOffsetPos(baseStats.groundCheckOffset),
            Vector2.down,
            baseStats.groundCheckDist,
            groundLayer
        );

        IsWallDetected = Physics2D.Raycast(
            GetOffsetPos(baseStats.wallCheckOffset),
            transform.right,
            baseStats.wallCheckDist,
            groundLayer
        );
    }

    private Vector2 GetOffsetPos(Vector2 offset)
    {
        return new Vector2(transform.position.x + transform.right.x * offset.x, transform.position.y + offset.y);
    }

    public void HandleFlip()
    {
        if (!shouldFlip) return;
        
        if ((transform.right.x > 0 && _input.x < 0f)
            || (transform.right.x < 0 && _input.x > 0f))
            transform.Rotate(0f, _flipScale, 0f);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(
            GetOffsetPos(baseStats.groundCheckOffset),
            GetOffsetPos(baseStats.groundCheckOffset) + new Vector2(0f, - baseStats.groundCheckDist)
        );

        Gizmos.DrawLine(
            GetOffsetPos(baseStats.wallCheckOffset),
            GetOffsetPos(baseStats.wallCheckOffset) + new Vector2(transform.right.x * baseStats.wallCheckDist, 0f)
        );
    }
#endif
}