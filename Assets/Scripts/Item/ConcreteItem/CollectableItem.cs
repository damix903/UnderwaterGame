using UnityEngine;

public class CollectableItem : Item, ICollectable
{
    public void Collect(GameObject instigator)
    {
        publisher?.Publish(EventPublisher.Others, new ItemEvent(itemData, instigator, transform));
        Release();
    }
}
