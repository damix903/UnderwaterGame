using UnityEngine;

public class PoolableEntityFactory<T> : IEntityFactory<T> where T : PoolableEntity
{
    private readonly ObjectPoolManager _poolManager;

    public PoolableEntityFactory(ObjectPoolManager poolManager)
    {
        _poolManager = poolManager;
    }
    
    public T Create(EntityData data, Transform spawnPoint)
    {
        var obj = _poolManager.Get<T>(data.Prefab);
        if (obj == null) return null;
        obj.transform.position = spawnPoint.position;
        obj.transform.rotation = spawnPoint.rotation;
        obj.Initialize(data);

        return obj;
    }
}