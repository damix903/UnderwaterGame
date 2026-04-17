using Manager.AudioSystem;
using ProjectileSystem;
using VContainer;
using VContainer.Unity;

namespace LifeTimeScope
{
    public class FactoryInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.Register<PoolableEntityFactory<Projectile>>(Lifetime.Singleton).As<IEntityFactory<Projectile>>();
            builder.Register<PoolableEntityFactory<Room>>(Lifetime.Singleton).As<IEntityFactory<Room>>();
            builder.Register<PoolableEntityFactory<Item>>(Lifetime.Singleton).As<IEntityFactory<Item>>();
            builder.Register<PoolableEntityFactory<SoundEmitter>>(Lifetime.Singleton).As<IEntityFactory<SoundEmitter>>();
            builder.Register<Room>(Lifetime.Transient);
            builder.Register<Item>(Lifetime.Transient);
        }
    }
}