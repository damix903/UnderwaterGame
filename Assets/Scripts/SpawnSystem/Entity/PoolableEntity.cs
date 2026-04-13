using System;
using MessagePipe;
using UnityEngine;
using VContainer;

public abstract class PoolableEntity : Entity, IPoolable
{
    [Header("Pool Settings")]
    [SerializeField] private int defaultCapacity = 10;
    [SerializeField] private int maxSize = 100;
    
    public int DefaultCapacity => defaultCapacity;
    public int MaxSize => maxSize;

    private Action _releaseAction;
    private bool _isActive;
    
    protected abstract ReleaseType ReleaseType { get; }
    private IDisposable _subscription;
    [Inject] private ISubscriber<ReleaseType> _subscriber;

    public void InitializePool(Action release)
    {
        _releaseAction = release;
        _isActive = true;
        OnSpawn();
    }
    
    protected virtual void OnSpawn(){}

    protected void Release()
    {
        if (!_isActive) return;
        
        _releaseAction?.Invoke();
        _isActive = false;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _subscription = _subscriber.Subscribe((type) =>
        {
            if (type == ReleaseType) Release();
        });
    }
    
    protected override void OnDisable()
    {
        base.OnDisable();
        _subscription?.Dispose();
    }
}