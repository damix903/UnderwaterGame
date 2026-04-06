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
    
    private IDamageable _damageable;
    private AIController _controller;

    protected override void Awake()
    {
        base.Awake();
        _damageable = GetComponent<IDamageable>();
        _controller = GetComponent<AIController>();
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        
        if (data is EnemyData enemyData)
            _controller.Initialize(enemyData);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (_damageable != null)
        {
            _damageable.OnDeath += HandleDeathEvent;
            _damageable.OnDamaged += HandleDamageEvent;
        }
        
        _subscription = _subscriber?.Subscribe((type) =>
        {
            if (type == ReleaseType.Enemy) Release();
        });
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (_damageable != null)
        {
            _damageable.OnDeath -= HandleDeathEvent;
            _damageable.OnDamaged -= HandleDamageEvent;
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