using System;
using UnityEngine;

public abstract class PoolableEntity : Entity, IPoolable
{
    [Header("Pool Settings")]
    [SerializeField] private int defaultCapacity = 10;
    [SerializeField] private int maxSize = 100;
    
    public int DefaultCapacity => defaultCapacity;
    public int MaxSize => maxSize;

    private Action _releaseAction;
    private bool _isActive;

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
    
}