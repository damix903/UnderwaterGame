using UnityEngine;
using System;
using MessagePipe;
using VContainer;

public class EntityHealth : MonoBehaviour, IDamageable
{
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected TeamID teamID;
    
    public float CurrentHealth { get; private set; }
    public TeamID TeamID => teamID;
    public bool IsAlive => CurrentHealth > 0;
    public DefenseState DefenseState { get; set; }

    private IPublisher<DeathEvent> _publisher;

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    [Inject]
    public void Construct(IPublisher<DeathEvent> publisher)
    {
        _publisher = publisher;
    }

    public virtual void AddHealth(float amount)
    {
        CurrentHealth += amount;
    }

    public bool TakeDamage(DamageInfo info)
    {
        if (DefenseState == DefenseState.Invincible) return false;
        
        OnDamaged(info);
        
        if (CurrentHealth <= 0)
            OnDeath(info);

        return true;
    }

    protected virtual void OnDamaged(DamageInfo info)
    {
        CurrentHealth -= info.Damage;
    }

    protected virtual void OnDeath(DamageInfo info)
    {
        _publisher?.Publish(new DeathEvent(gameObject, teamID, info));
        Destroy(gameObject);
    }
}