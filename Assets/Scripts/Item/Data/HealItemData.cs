using System.Collections.Generic;
using Stat;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Entity/Item/Heal")]
public class HealItemData : ItemData
{
    [SerializeField] private float healAmount;
    
    public override void ApplyEffect(ItemManager manager, ItemEvent e)
    {
        if (e.Target.TryGetComponent<IHealth>(out var health))
        {
            health.ChangeHealth(healAmount);
        }
    }
}
