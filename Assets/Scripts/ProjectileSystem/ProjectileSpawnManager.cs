using UnityEngine;

namespace ProjectileSystem
{
    public interface IProjectileService
    {
        Projectile Spawn(EntityData data, Transform spawnPoint);
    }

    public class ProjectileSpawnManager : IProjectileService
    {
        private IEntityFactory<Projectile>  _factory;
        
        public ProjectileSpawnManager(IEntityFactory<Projectile> factory)
        {
            _factory = factory;
        }

        public Projectile Spawn(EntityData data, Transform spawnPoint)
        {
            var proj = _factory.Create(data, spawnPoint);
            return proj;
        }
    }
}