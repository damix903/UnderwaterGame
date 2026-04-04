using MessagePipe;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class Item : PoolableEntity
{
    [Inject] protected IPublisher<EventPublisher, ItemEvent> publisher;
    protected ItemData itemData;
    
    private SpriteRenderer sr;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        
        sr = GetComponent<SpriteRenderer>();
        itemData = (ItemData)data;
        
        if (itemData.Sprite != null)
            sr.sprite = itemData.Sprite;
    }
}