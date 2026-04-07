using System;
using MessagePipe;
using Stat;
using UnityEngine;
using VContainer;

public class Player : MonoBehaviour
{
    [Inject] private IPublisher<EventPublisher, HealthChangeEvent> _healthPub;
    [Inject] private IPublisher<EventPublisher, DamageResult> _damagePub;
    [Inject] private IPublisher<EventPublisher, LandedEvent> _landedPub;
    private IDamageable _damageable;
    private IHealth _health;
    private ICollisionDetectable _collisionDetectable;

    private void Awake()
    {
        _damageable = GetComponent<IDamageable>();
        _health = GetComponent<IHealth>();
        _collisionDetectable = GetComponent<ICollisionDetectable>();
    }

    private void OnEnable()
    {
        _health.OnHealthChanged += HandleHealthChange;
        _damageable.OnDamaged += HandleDamage;
        _collisionDetectable.OnLanded += HandleLanded;
    }

    private void OnDisable()
    {
        if (_health != null)
            _health.OnHealthChanged -= HandleHealthChange;
        
        if (_collisionDetectable != null)
            _collisionDetectable.OnLanded -= HandleLanded;
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