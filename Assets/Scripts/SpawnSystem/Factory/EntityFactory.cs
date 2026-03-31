using UnityEngine;

public class EntityFactory<T> : IEntityFactory<T> where T : Entity
{
    public T Create(EntityData data, Transform spawnPoint)
    {
        var obj = GameObject.Instantiate(data.Prefab, spawnPoint.position, spawnPoint.rotation);

        if (!obj.TryGetComponent(out T entity))
        {
            entity = obj.AddComponent<T>();
        }
        
        entity.transform.position = spawnPoint.position;
        entity.transform.rotation = spawnPoint.rotation;
        entity.Initialize(data);

        return entity;
    }
}