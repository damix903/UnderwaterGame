using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Entity/Item/Heal")]
public class HealItemData : ItemData
{
    [SerializeField] private float healAmount;
    
    public override void ApplyEffect(ItemManager manager, ItemEvent e)
    {
        if (e.Target.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.Heal(healAmount);
        }
    }
}
