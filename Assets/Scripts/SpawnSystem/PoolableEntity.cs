using System;
using UnityEngine;

public abstract class PoolableEntity : Entity, IPoolable
{
    [SerializeField] private int defaultCapacity = 10;
    [SerializeField] private int maxSize = 100;
    
    public int DefaultCapacity => defaultCapacity;
    public int MaxSize => maxSize;

    private Action _releaseAction;

    public void InitializePool(Action release)
    {
        _releaseAction = release;
        OnSpawn();
    }
    
    protected virtual void OnSpawn(){}
    protected void Release() => _releaseAction?.Invoke();
}