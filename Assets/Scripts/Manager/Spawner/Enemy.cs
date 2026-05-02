using System;
using EnemyAI;
using EnemyAI.Move;
using MessagePipe;
using Movement;
using Sensor;
using Stage;
using Underwater.Utility.Timer;
using UnityEngine;
using Utility;
using VContainer;
using Debug = UnityEngine.Debug;

public class Enemy : PoolableEntity
{
    [Inject] private ICollisionConfig _collisionConfig;
    [Inject] private EnemyMessageBroker _messageBroker;
    [Inject] private ITimerService _timerService;

    [SerializeReference, SubclassSelector] private IDetectable _detectable;
    
    private AIController _controller;
    private EntityHealth _health;

    private EnemyContext _ctx;
    private Camera _cam;
    private ITimerHandle _timerHandle;
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
        
        _cam = Camera.main;
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();

        if (data is EnemyData enemyData) _ctx.SetData(enemyData);
        else Debug.LogError("EnemyData is not set for Enemy.");

        if (gameObject.IsInCameraView(_cam, .2f))
            Initialize();

        _timerHandle = _timerService.SetTimer(.2f, () =>
        {
            if (_initialized)
            {
                _timerHandle.Dispose();
                return;
            }
            if (gameObject.IsInCameraView(_cam, .2f)) Initialize();
        }, true).BindTo(gameObject);
    }
    
    private bool _initialized = false;

    private void Initialize()
    {
        _health.Initialize(_ctx.Data.MaxHealth, TeamID.Enemy);
        _ctx.Anim.Initialize(_ctx.EventListenable, _ctx.Data.AnimData);

        _controller.Initialize(_ctx);
        _initialized = true;
        Debug.Log("Enemy initialized: " + gameObject.name);
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
        _initialized = false;
        _timerHandle.Dispose();
    }

    private void HandleDamageEvent(DamageResult result)
    {
        _messageBroker?.Publish(result);
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