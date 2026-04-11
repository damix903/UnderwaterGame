using System;
using Manager.UpGrade;
using MessagePipe;
using PlayerSystem;
using Stat;
using UnityEngine;
using VContainer;

public class Player : MonoBehaviour
{
    [Inject] private IPublisher<EventPublisher, HealthChangeEvent> _healthPub;
    [Inject] private IPublisher<EventPublisher, DamageResult> _damagePub;
    [Inject] private IPublisher<EventPublisher, LandedEvent> _landedPub;
    
    [Inject] private IPlayerRegisterable _playerRegisterable;
    
    private IDamageable _damageable;
    private IHealth _health;
    private ICollisionDetectable _collisionDetectable;

    private void Awake()
    {
        _damageable = GetComponent<IDamageable>();
        _health = GetComponent<IHealth>();
        _collisionDetectable = GetComponent<ICollisionDetectable>();
    }
    
    public void ApplyRunState(RunState runState)
    {
        // debug runstate upgrade
        foreach (var upGrade in runState.UpGradeList)
            Debug.Log($"Player received upgrade: {upGrade.UpgradeName}");
    }

    private void OnEnable()
    {
        _health.OnHealthChanged += HandleHealthChange;
        _damageable.OnDamaged += HandleDamage;
        _collisionDetectable.OnLanded += HandleLanded;
        _playerRegisterable?.RegisterPlayer(this);
    }

    private void OnDisable()
    {
        if (_health != null)
            _health.OnHealthChanged -= HandleHealthChange;
        
        if (_collisionDetectable != null)
            _collisionDetectable.OnLanded -= HandleLanded;
        
        if (_damageable != null)
            _damageable.OnDamaged -= HandleDamage;
        
        _playerRegisterable?.UnregisterPlayer();
    }

    private void HandleDamage(DamageResult result) 
        =>_damagePub.Publish(EventPublisher.Player, result);

    private void HandleLanded() 
        => _landedPub.Publish(EventPublisher.Player, new LandedEvent());

    private void HandleHealthChange(HealthChangeEvent obj) 
        => _healthPub?.Publish(EventPublisher.Player, obj);
}

public struct LandedEvent
{
    
}