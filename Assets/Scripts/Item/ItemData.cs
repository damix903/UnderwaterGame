using System.Collections.Generic;
using UnityEngine;

public abstract class ItemData : EntityData
{
    [SerializeField] private Sprite sprite;
    
    public Sprite Sprite => sprite;
    
    public abstract void ApplyEffect(ItemManager manager, ItemEvent e);
}
