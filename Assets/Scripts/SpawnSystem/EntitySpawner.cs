
// public class EntitySpawner<T> where T : Entity
// {
//     private readonly IEntityFactory<T> _factory;
//     private readonly ISpawnStrategy _spawnStrategy;
//
//     public EntitySpawner(IEntityFactory<T> factory, ISpawnStrategy spawnStrategy)
//     {
//         _factory = factory;
//         _spawnStrategy = spawnStrategy;
//     }
//
//     public T Create()
//     {
//         return _factory.Create(_spawnStrategy.GetSpawnPoint());
//     }
// }