using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Entity/Item/Spawn")]
public class SpawnItemData : ItemData
{
    [SerializeField] private List<ItemData> spawnItems;
    
    public override void ApplyEffect(ItemManager manager, ItemEvent e)
    {
        foreach (var i in spawnItems)
        {
            manager.Spawn(i, e.Point);
        }
    }
}
