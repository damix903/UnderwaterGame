using UnityEngine;

namespace SpawnSystem
{
    public interface IEntityFactory<out T> where T : Entity
    {
        public T Create(EntityData data, SpawnPoint point);
    }

    public struct SpawnPoint
    {
        public Vector3 position;
        public Quaternion rotation;
        
        public SpawnPoint(Vector3 position, Quaternion rotation = default)
        {
            this.position = position;
            this.rotation = rotation;
        }
    }
}