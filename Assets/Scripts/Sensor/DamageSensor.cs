using System;
using UnityEngine;
using VContainer;

public class DamageSensor : IDetectable, IDisposable
{
    public event Action<GameObject> OnTargetDetected;
    public event Action<GameObject> OnTargetLost;

    private IDamageable _damageable;
    
    [Inject]
    public void Construct(IDamageable damageable)
    {
        _damageable = damageable;
    }

    private void HandleDamage(DamageResult result)
    {
        var attacker = result.DamageInfo.Attacker;
        if (attacker != null) OnTargetDetected?.Invoke(attacker);
    }

    public void Dispose()
    {
        _damageable.OnDamaged -= HandleDamage;
    }
}