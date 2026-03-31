using UnityEngine;

public interface IEntityFactory<T> where T : Entity
{
    public T Create(EntityData data, Transform spawnPoint);
}