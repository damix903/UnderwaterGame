using System;
using MessagePipe;
using Stat;
using UnityEngine;
using VContainer;

public class Player : MonoBehaviour
{
    [Inject] private IPublisher<EventPublisher, HealthChangeEventArgs> _publisher;
    private IDamageable _damageable;
    private IHealth _health;

    private void Awake()
    {
        _damageable = GetComponent<IDamageable>();
        _health = GetComponent<IHealth>();
    }

    private void OnEnable()
    {
        _health.OnHealthChanged += HandleHealthChange;
    }

    private void HandleHealthChange(HealthChangeEventArgs obj)
    {
        _publisher?.Publish(EventPublisher.Player, obj);
    }
}