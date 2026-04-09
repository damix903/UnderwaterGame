using UnityEngine;
using System;
using Stat;
using MessagePipe;
using VContainer;

public class EntityHealth : MonoBehaviour, IDamageable, IHealth
{
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected TeamID teamID;
    
    public float CurrentHealth { get; private set; }
    public float MaxHealth => maxHealth;
    public event Action<HealthChangeEvent> OnHealthChanged;
    
    public TeamID TeamID => teamID;
    public bool IsAlive => CurrentHealth > 0;
    public DefenseState DefenseState { get; set; }
    public event Action<DamageResult> OnDamaged;
    public event Action<DeathEvent> OnDeath;

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void Initialize(float maxHealth, TeamID teamID)
    {
        this.maxHealth = maxHealth;
        this.teamID = teamID;
        ChangeHealth(maxHealth);
    }
    
    public bool TakeDamage(DamageInfo info)
    {
        if (!IsAlive) return false;
        if (DefenseState == DefenseState.Invincible) return false;
        
        OnDamageTaken(info);
        
        if (CurrentHealth <= 0)
            HandleDeath(info);

        return true;
    }

    public void ChangeHealth(float amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, maxHealth);
        OnHealthChanged?.Invoke(new HealthChangeEvent(CurrentHealth, maxHealth, amount));
    }

    protected virtual void OnDamageTaken(DamageInfo info)
    {
        ChangeHealth(-info.Damage);
        var result = new DamageResult
        {
            DamageInfo = info,
            AppliedDamage = info.Damage,
            Defender = gameObject,
        };
        
        OnDamaged?.Invoke(result);
    }

    protected virtual void HandleDeath(DamageInfo info)
    {
        var e = new DeathEvent(gameObject, teamID, info);
        OnDeath?.Invoke(e);
    }
}