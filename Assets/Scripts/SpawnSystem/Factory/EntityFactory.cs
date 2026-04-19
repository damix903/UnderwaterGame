using UnityEngine;
using Utility;

namespace SpawnSystem
{
    public class EntityFactory<T> : IEntityFactory<T> where T : Entity
    {
        public T Create(EntityData data, SpawnPoint point)
        {
            var obj = GameObject.Instantiate(data.Prefab, point.position, point.rotation);

            var entity = obj.GetOrAddComponent<T>();
        
            entity.transform.position = point.position;
            entity.transform.rotation = point.rotation;
            entity.Initialize(data);

            return entity;
        }
    }
}