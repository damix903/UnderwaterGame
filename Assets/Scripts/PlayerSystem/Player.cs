using System;
using Animation;
using Manager.Upgrade;
using MessagePipe;
using PlayerSystem;
using Stat;
using UnityEngine;
using VContainer;

public class Player : MonoBehaviour
{
    [SerializeField] private float invincibleDuration = 1f;
    
    [Inject] private IPublisher<EventPublisher, HealthChangeEvent> _healthPub;
    [Inject] private IPublisher<EventPublisher, DamageResult> _damagePub;
    [Inject] private IPublisher<DamageResult> _damageResultPub;
    [Inject] private IPublisher<EventPublisher, DeathEvent> _deathPub;

    [Inject] private IPlayerRegisterable _playerRegisterable;
    [Inject] private ILayerConfig _layerConfig;
    
    private IDamageable _damageable;
    private IHealth _health;
    private InvincibleHandler _invincibleHandler;
    
    private void Awake()
    {
        _damageable = GetComponent<IDamageable>();
        _health = GetComponent<IHealth>();

        _invincibleHandler = new InvincibleHandler.Builder(gameObject, _damageable)
            .WithDuration(invincibleDuration)
            .WithBlinker(GetComponentsInChildren<ISpriteBlinker>())
            .WithInvincibleLayer(_layerConfig.InvincibleLayer)
            .Build();
    }
    
    public void ApplyRunState(RunState runState)
    {
        // debug runstate upgrade
        // foreach (var upGrade in runState.UpGradeList)
        //     Debug.Log($"Player received upgrade: {upGrade.UpgradeName}");
    }

    private void OnEnable()
    {
        _health.OnHealthChanged += HandleHealthChange;
        _damageable.OnDamaged += HandleDamage;
        _damageable.OnDeath += HandleDeath;
        _playerRegisterable?.RegisterPlayer(this);
    }


    private void OnDisable()
    {
        if (_health != null) _health.OnHealthChanged -= HandleHealthChange;

        if (_damageable != null) _damageable.OnDamaged -= HandleDamage;
        
        if (_damageable != null) _damageable.OnDeath -= HandleDeath;
        
        _playerRegisterable?.UnregisterPlayer();
    }

    private void HandleDamage(DamageResult result)
    {
        _damagePub?.Publish(EventPublisher.Player, result);
        _damageResultPub?.Publish(result);
    }

    private void HandleHealthChange(HealthChangeEvent obj) 
        => _healthPub?.Publish(EventPublisher.Player, obj);

    private void HandleDeath(DeathEvent e)
    {
        _deathPub?.Publish(EventPublisher.Player, e);
    }
}