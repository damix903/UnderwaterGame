using System;
using UnityEngine;

public class DestructiveItem : Item, IDamageable
{
    public TeamID TeamID => TeamID.Others;
    public bool IsAlive => gameObject.activeInHierarchy;
    public DefenseState DefenseState { get; set; }
    
    public bool TakeDamage(DamageInfo info)
    {
        if (!CanBeDestroyed(info)) return false;
        
        publisher?.Publish(EventPublisher.Others, new ItemEvent(itemData, info.Attacker, transform));
        Release();
        
        return true;
    }

    private bool CanBeDestroyed(DamageInfo info)
    {
        return IsAlive
               && info.Attacker.TryGetComponent<IDamageable>(out var damageable)
               && damageable.TeamID == TeamID.Player;
    }

    public void Heal(float amount) { }
    public event Action<DamageResult> OnDamaged;
    public event Action<DeathEvent> OnDeath;
}
