using System;
using MessagePipe;
using UnityEngine;
using VContainer;

public class PlayerHealth : EntityHealth
{
    [Inject] private IPublisher<EventPublisher, HealthChangeEventArgs> _publisher;
    [Inject] private ISubscriber<DeathEvent> _subscriber;

    private IDisposable _subscription;
    
    private void Start()
    {
        _subscription = _subscriber?.Subscribe(HandelDeathEvent);
    }

    private void OnDestroy()
    {
        _subscription?.Dispose();
    }

    private void HandelDeathEvent(DeathEvent e)
    {
        AddHealth(5f);
    }

    public override void AddHealth(float amount)
    {
        base.AddHealth(amount);
        PublishHealthChange(amount);
    }

    protected override void OnDamaged(DamageInfo info)
    {
        base.OnDamaged(info);
        PublishHealthChange(-info.Damage);
    }

    private void PublishHealthChange(float amount)
    {
        _publisher?.Publish(EventPublisher.Player, new HealthChangeEventArgs(
            CurrentHealth,
            maxHealth,
            amount
        ));
    }
}

public struct HealthChangeEventArgs
{
    public readonly float Current;
    public readonly float Max;
    public readonly float ChangedAmount;

    public HealthChangeEventArgs(float current, float max, float changedAmount)
    {
        Current = current;
        Max = max;
        ChangedAmount = changedAmount;
    }
}