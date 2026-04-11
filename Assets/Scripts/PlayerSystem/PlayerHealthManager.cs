using System;
using MessagePipe;
using Stat;
using UnityEngine;
using VContainer.Unity;

public interface ICostable
{
    void Recover(float amount, int comboCount = 0);
    void Consume(float amount);
}

public class EmptyCostable : ICostable
{
    public void Recover(float amount, int comboCount = 0) { }
    public void Consume(float amount) { }
}

public class PlayerHealthManager : IDisposable, ITickable, ICostable
{
    private IHealth _health;
    private IDisposable _subscription;

    private float _consumeAmountPerSecond;
    private float _timer;
    private const float ConsumeTime = 1f;
    
    public PlayerHealthManager(IHealth health, ISubscriber<ComboEvent> comboSub)
    {
        _health = health;
        _subscription = comboSub?.Subscribe(HandleComboEvent);
        _timer = ConsumeTime;
        _consumeAmountPerSecond = 1f;
    }

    private void HandleComboEvent(ComboEvent e)
    {
        var amount = 5f;
        if (e.Count >= 5)
        {
            amount += e.Count / 3f;
            amount = Mathf.Min(amount, 10f);
        }
        
        Recover(amount);
    }
    
    public void SetConsumeAmount(float amount) => _consumeAmountPerSecond = amount;

    public void Recover(float amount, int comboCount = 0)
    {
        _health.ChangeHealth(amount);
    }

    public void Consume(float amount)
    {
        _health.ChangeHealth(-amount);
    }
    
    public void Dispose()
    {
        _subscription?.Dispose();
    }

    public void Tick()
    {
        if (_health == null) return;
        
        _timer -= Time.deltaTime;
        if (_timer < 0f)
        {
            Consume(_consumeAmountPerSecond);
            _timer = _consumeAmountPerSecond;
        }
    }
}