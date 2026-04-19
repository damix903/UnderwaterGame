using SpawnSystem;
using UnityEngine;

namespace ProjectileSystem
{
    public interface IProjectileService
    {
        Projectile Spawn(BaseProjectileData data, SpawnPoint point);
    }

    public class ProjectileSpawnManager : IProjectileService
    {
        private IEntityFactory<Projectile>  _factory;
        
        public ProjectileSpawnManager(IEntityFactory<Projectile> factory)
        {
            _factory = factory;
        }

        public Projectile Spawn(BaseProjectileData data, SpawnPoint point)
        {
            var proj = _factory.Create(data, point);
            return proj;
        }
    }
}