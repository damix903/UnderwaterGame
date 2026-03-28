using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField] private List<PoolData> poolConfigs;

    private readonly Dictionary<PoolType, IObjectPool<GameObject>> _pools = new();

    private void Awake()
    {
        foreach (var data in poolConfigs)
        {
            var pool = new ObjectPool<GameObject>(
                createFunc: () => Instantiate(data.prefab),
                actionOnGet: (obj) => obj.SetActive(true),
                actionOnRelease: (obj) => obj.SetActive(false),
                actionOnDestroy: (obj) => Destroy(obj),
                collectionCheck: true,
                defaultCapacity: data.defaultCapacity,
                maxSize: data.maxSize
            );
            
            _pools.Add(data.poolType, pool);
        }
    }

    public GameObject Get(PoolType poolType)
    {
        if (!_pools.TryGetValue(poolType, out var pool)) return null;
        
        var obj = pool.Get();
        if (obj.TryGetComponent<IPoolable>(out var poolable))
        {
            poolable.InitializePool(() => pool.Release(obj));
        }

        return obj;
    }
}

[Serializable]
public struct PoolData
{
    public PoolType poolType;
    public GameObject prefab;
    public int defaultCapacity;
    public int maxSize;
}

public enum PoolType {VFX, Projectile}