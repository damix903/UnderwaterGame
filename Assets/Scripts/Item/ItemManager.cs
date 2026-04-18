using System;
using MessagePipe;
using SpawnSystem;
using UnityEngine;
using VContainer;

public class ItemManager : MonoBehaviour
{
    [Inject] private IEntityFactory<Item> _factory;
    private IDisposable _subscription;

    [Inject]
    public void Construct(ISubscriber<EventPublisher, ItemEvent> subscriber)
    {
        _subscription = subscriber?.Subscribe(EventPublisher.Others, HandleItemEvent);
    }

    private void HandleItemEvent(ItemEvent e)
    {
        if (e.Data == null || e.Target == null) return;
        
        e.Data.ApplyEffect(this, e);
    }

    public void Spawn(ItemData data, Transform point)
    {
        _factory.Create(data, new SpawnPoint(point.position, point.rotation));
    }

    private void OnDestroy()
    {
        _subscription?.Dispose();
    }
}

public struct ItemEvent
{
    public ItemData Data;
    public GameObject Target;
    public Transform Point;
    
    public ItemEvent(ItemData data, GameObject target, Transform point)
    {
        Data = data;
        Target = target;
        Point = point;
    }
}