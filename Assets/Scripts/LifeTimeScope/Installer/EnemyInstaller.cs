using EnemyAI;
using ProjectileSystem;
using VContainer;
using VContainer.Unity;

namespace LifeTimeScope
{
    public class EnemyInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.Register<PoolableEntityFactory<Enemy>>(Lifetime.Singleton).As<IEntityFactory<Enemy>>();
            builder.Register<Enemy>(Lifetime.Transient);
            builder.Register<EmptyCostable>(Lifetime.Singleton).AsImplementedInterfaces(); // コストなしなのでシングルトンで十分
            builder.Register<EnemyMessageBroker>(Lifetime.Transient);
            builder.Register<EmptyProjModifierProvider>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}