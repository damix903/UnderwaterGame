using System;
using EnemyAI;
using MessagePipe;
using Movement;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;

public class Enemy : PoolableEntity
{
    [Inject] private ICollisionConfig _collisionConfig;
    [Inject] private EnemyMessageBroker _messageBroker;

    private AIController _controller;
    private EntityHealth _health;
    
    private EnemyContext _ctx;
    protected override ReleaseType ReleaseType => ReleaseType.Enemy;

    protected override void Awake()
    {
        base.Awake();
        _controller = GetComponent<AIController>();
        _health = GetComponent<EntityHealth>();
        
        _ctx = new EnemyContext.Builder()
            .WithMovement(GetComponent<CharacterMovement>())
            .WithAnim(GetComponentInChildren<IAnimPlayable>())
            .WithEventListenable(GetComponentInChildren<IAnimEventListenable>())
            .Build();
    }

    // when i use object pool, dependency injection should bd done every time on spawn?
    //
    protected override void OnInitialize()
    {
        base.OnInitialize();

        if (data is EnemyData enemyData)
        {
            _ctx.SetData(enemyData);
            _controller.Initialize(_ctx);
            _health.Initialize(enemyData.MaxHealth, TeamID.Enemy);
            _ctx.Anim.Initialize(_ctx.EventListenable, enemyData.AnimData);
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
        _messageBroker.OnRelease += HandleRelease;
    }

    private void HandleRelease(ReleaseType obj)
    {
        if (obj == ReleaseType.Enemy) Release();
        Debug.Log("Enemy received release message: " + obj);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (_health != null)
        {
            _health.OnDeath -= HandleDeathEvent;
            _health.OnDamaged -= HandleDamageEvent;
        }
        
        _messageBroker.OnRelease -= HandleRelease;
    }

    private void HandleDamageEvent(DamageResult result)
    {
    }

    private void HandleDeathEvent(DeathEvent e)
    {
        _messageBroker?.Publish(e);
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