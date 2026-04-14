using System;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterMovement : MonoBehaviour, ICollisionDetectable, IForceApplicable
    {

        [SerializeField] private bool shouldFlip = true;
        [SerializeField] private bool isFacingRight = true;
    
        [Header("Collision Check")]
        [SerializeField] private LayerMask groundLayer;
    
        [SerializeField] private BaseMovementStats baseStats;
    
        #region Private Fields
    
        private Rigidbody2D _rb;
        private IHandleInputStrategy _strategy;
    
        private enum MovementMode { None, Normal}
        private MovementMode _movementMode = MovementMode.Normal;
    
        private MovementRuntimeStats _currentStats;
        private readonly List<IMovementModifier> _modifiers = new();
        private bool _isDirty;
    
        private float _flipScale;
        private float _movementBlockTimer;
        private bool _shouldBlockY;
        private float _inputMul;
        private bool _isOverwriteX, _isOverwriteY;
    
        private Vector2 _input;
        private Vector2 _frameVelocity;
        private Vector2 _impulseVelocity;
        private Vector2 _constantVelocity;
    
        #endregion

        public Vector2 Velocity => _rb.linearVelocity;
        public bool IsGrounded { get; private set; }
        public bool IsWallDetected { get; private set; }
        public event Action OnLanded;
        public event Action OnWallDetected;
        public bool IsFalling => !IsGrounded && _rb.linearVelocity.y < 0f;
    
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            if (_rb == null) _rb = gameObject.AddComponent<Rigidbody2D>();
            _rb.gravityScale = 0f;
            _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            _rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            _rb.freezeRotation = true;

            _flipScale = isFacingRight ? 180f : -180f;

            _strategy = baseStats.Strategy();
            _isDirty = true;
        }

        public void SetMovementInput(Vector2 input) => _input = input;

        public void AddModifier(IMovementModifier modifier)
        {
            _modifiers.Add(modifier);
            _isDirty = true;
        }

        public void RemoveModifier(IMovementModifier modifier)
        {
            _modifiers.Remove(modifier);
            _isDirty = true;
        } 

        public void AddImpulseForce(Vector2 force, bool overwriteX = false, bool overwriteY = false)
        {
            _impulseVelocity = force;
            _isOverwriteX = overwriteX;
            _isOverwriteY = overwriteY;
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
            if (_isDirty)
            {
                _currentStats = baseStats.Stats;
                foreach (var m in _modifiers)
                    m.Apply(ref _currentStats);

                _isDirty = false;
            }

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
            //_rb.gravityScale = gravity;
        
            finalVel.x = Mathf.Clamp(finalVel.x, -_currentStats.maxSpeed, _currentStats.maxSpeed);
            finalVel.y = Mathf.Clamp(finalVel.y, -_currentStats.maxFallSpeed, _currentStats.maxFallSpeed);
        
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
        }

        private void HandleImpulseForce()
        {
            if (_impulseVelocity == Vector2.zero) return;

            _frameVelocity.x = _isOverwriteX ? _impulseVelocity.x : _frameVelocity.x + _impulseVelocity.x;
            _frameVelocity.y = _isOverwriteY ? _impulseVelocity.y : _frameVelocity.y + _impulseVelocity.y;
            _impulseVelocity = Vector2.zero;
        }
    
        private void HandleCollisionDetection()
        {
            bool currentGrounded = Physics2D.Raycast(
                GetOffsetPos(baseStats.groundCheckOffset),
                Vector2.down,
                baseStats.groundCheckDist,
                groundLayer
            );

            bool currentWallDetected = Physics2D.Raycast(
                GetOffsetPos(baseStats.wallCheckOffset),
                transform.right,
                baseStats.wallCheckDist,
                groundLayer
            );
        
            if (currentGrounded && !IsGrounded)
                OnLanded?.Invoke();

            if (currentWallDetected && !IsWallDetected)
                OnWallDetected?.Invoke();
        
            IsGrounded = currentGrounded;
            IsWallDetected = currentWallDetected;
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
}