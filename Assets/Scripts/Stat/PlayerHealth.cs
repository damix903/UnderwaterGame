using System;
using MessagePipe;
using UnityEngine;
using VContainer;

public class PlayerHealth : EntityHealth
{
    [Inject] private IPublisher<EventPublisher, HealthChangeEventArgs> _publisher;

    private IDisposable _subscription;

    [Inject]
    public void Construct(ISubscriber<DeathEvent> deathEvent, ISubscriber<EventPublisher, ComboEvent> comboEvent)
    {
        var bag = DisposableBag.CreateBuilder();
        deathEvent?.Subscribe(HandelDeathEvent).AddTo(bag);
        comboEvent?.Subscribe(EventPublisher.System, HandleComboEvent).AddTo(bag);
        _subscription = bag.Build();
    }

    private void Start()
    {
        ChangeHealth(maxHealth);
    }

    private void HandleComboEvent(ComboEvent e)
    {
        if (e.Count < 5) return;
        var amount = e.Count / 3f;
        amount = Mathf.Min(amount, 10f);
        Heal(amount);
    }

    private void OnDestroy()
    {
        _subscription?.Dispose();
    }

    private void HandelDeathEvent(DeathEvent e)
    {
        ChangeHealth(5f);
    }

    protected override void ChangeHealth(float amount)
    {
        base.ChangeHealth(amount);
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