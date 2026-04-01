using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;
using VContainer.Unity;


public class ObjectPoolManager : MonoBehaviour
{
    private readonly Dictionary<string, IObjectPool<GameObject>> _pools = new();
    [Inject] private IObjectResolver _resolver;
    
    public T Get<T>(GameObject prefab) where T : MonoBehaviour, IPoolable
    {
        if (!_pools.TryGetValue(prefab.name, out var pool))
        {
            pool = CreatePool(prefab);
            if (pool == null) return null;
        }
        
        var obj = pool.Get();

        if (!obj.TryGetComponent<T>(out var poolable))
        {
            Debug.LogError($"{prefab.name} does not have an IPoolable");
            return null;
        }

        poolable.InitializePool(() => pool.Release(obj));
        return poolable;
    }

    public void ClearPool(GameObject prefab)
    {
        if (!_pools.TryGetValue(prefab.name, out var pool)) return;
        
        pool.Clear();
        _pools.Remove(prefab.name);
    }
    
    public void ClearAllPools() => _pools.Clear();

    private IObjectPool<GameObject> CreatePool(GameObject prefab)
    {
        if (!prefab.TryGetComponent<IPoolable>(out var poolable))
        {
            Debug.LogError($"{prefab.gameObject.GetType()} does not have an IPoolable");
            return null;
        }
            
        var pool = new ObjectPool<GameObject>(
            createFunc: () => _resolver.Instantiate(prefab),
            actionOnGet: (obj) => obj.SetActive(true),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: poolable.DefaultCapacity,
            maxSize: poolable.MaxSize
        );
        
        _pools.Add(prefab.name, pool);
        return pool;
    }
}