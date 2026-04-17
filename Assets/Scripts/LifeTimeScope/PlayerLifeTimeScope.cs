using Animation;
using ProjectileSystem;
using VContainer;
using VContainer.Unity;

namespace LifeTimeScope
{
    public class PlayerLifeTimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            //builder.RegisterComponentInHierarchy<PlayerHealth>().UnderTransform(transform).AsImplementedInterfaces()
            //  .AsSelf();

            builder.RegisterComponentInHierarchy<ProjectileShooter>().UnderTransform(transform);
            builder.Register<ProjModifierProvider>(Lifetime.Scoped).AsImplementedInterfaces();
        
            builder.RegisterComponentInHierarchy<PlayerController>().UnderTransform(transform);
            builder.RegisterComponentInHierarchy<AnimParamHandler>().UnderTransform(transform);
            builder.RegisterComponentInHierarchy<Player>().UnderTransform(transform);

            builder.Register<PlayerHealthManager>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.RegisterComponentInHierarchy<EntityHealth>().UnderTransform(transform).AsImplementedInterfaces();
            
            // var blinkers = GetComponentsInChildren<SpriteBlinker>(true);
            // foreach (var b in blinkers)
            //     builder.RegisterComponent(b).AsImplementedInterfaces();
                //builder.RegisterComponentInHierarchy<SpriteBlinker>().UnderTransform(transform).AsImplementedInterfaces();
        }
    }
}