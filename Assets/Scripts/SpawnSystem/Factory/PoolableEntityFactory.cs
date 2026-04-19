namespace SpawnSystem
{
    public class PoolableEntityFactory<T> : IEntityFactory<T> where T : PoolableEntity
    {
        private readonly ObjectPoolManager _poolManager;

        public PoolableEntityFactory(ObjectPoolManager poolManager)
        {
            _poolManager = poolManager;
        }
    
        public T Create(EntityData data, SpawnPoint point)
        {
            var obj = _poolManager.Get<T>(data.Prefab);
            if (obj == null) return null;
        
            obj.transform.position = point.position;
            obj.transform.rotation = point.rotation;
            obj.Initialize(data);

            return obj;
        }
    }
}