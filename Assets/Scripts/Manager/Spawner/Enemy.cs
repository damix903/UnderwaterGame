using System;
using MessagePipe;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;

public class Enemy : PoolableEntity
{
    [Inject] private ISubscriber<ReleaseType> _subscriber;
    [Inject] private ICollisionConfig _collisionConfig;
    [Inject] private IPublisher<DamageResult> _damagePub;
    [Inject] private IPublisher<DeathEvent> _deathPub;
    
    private IDisposable _subscription;

    private AIController _controller;
    private EntityHealth _health;
    private CharacterMovement _movement;
    private AnimationSystem _anim;
    private IAnimEventListenable _listener;

    protected override void Awake()
    {
        base.Awake();
        _controller = GetComponent<AIController>();
        _health = GetComponent<EntityHealth>();
        _movement = GetComponent<CharacterMovement>();
        _anim = GetComponentInChildren<AnimationSystem>();
        _listener = GetComponentInChildren<IAnimEventListenable>();
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();

        if (data is EnemyData enemyData)
        {
            var ctx = new EnemyContext(enemyData, _movement, _anim, _listener);
            _controller.Initialize(ctx);
            _health.Initialize(enemyData.MaxHealth, TeamID.Enemy);
            _anim.Initialize(_listener);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (_health != null)
        {
            _health.OnDeath += HandleDeathEvent;
            _health.OnDamaged += HandleDamageEvent;
        }
        
        _subscription = _subscriber?.Subscribe((type) =>
        {
            if (type == ReleaseType.Enemy) Release();
        });
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (_health != null)
        {
            _health.OnDeath -= HandleDeathEvent;
            _health.OnDamaged -= HandleDamageEvent;
        }
        
        _subscription?.Dispose();
    }

    private void HandleDamageEvent(DamageResult result)
    {
        
    }

    private void HandleDeathEvent(DeathEvent e)
    {
        _deathPub?.Publish(e);
        Release();
    }

    protected override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
        if (!other.gameObject.TryGetComponent<IDamageable>(out var damageable)) return;
        
        if (damageable.TeamID == TeamID.Player)
            damageable.TakeDamage(new DamageInfo(gameObject, _collisionConfig.Damage, _collisionConfig.EffectData));
    }
}