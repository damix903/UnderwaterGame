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

    public event Action<DamageResult> OnDamaged;
    public event Action<DeathEvent> OnDeath;

    private IPublisher<DamageResult> _damagePub;
    private IPublisher<DeathEvent> _deathPub;

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    [Inject]
    public void Construct(IPublisher<DeathEvent> publisher)
    {
        _deathPub = publisher;
    }
    
    public bool TakeDamage(DamageInfo info)
    {
        if (DefenseState == DefenseState.Invincible) return false;
        
        OnDamageTaken(info);
        
        if (CurrentHealth <= 0)
            HandleDeath(info);

        return true;
    }
    public void Heal(float amount) => ChangeHealth(amount);
    
    protected virtual void ChangeHealth(float amount) => CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, maxHealth);

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
        _damagePub?.Publish(result);
    }

    protected virtual void HandleDeath(DamageInfo info)
    {
        var e = new DeathEvent(gameObject, teamID, info);
        OnDeath?.Invoke(e);
        _deathPub?.Publish(e);
        Destroy(gameObject);
    }
}