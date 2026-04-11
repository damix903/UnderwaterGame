using ProjectileSystem;
using UnityEngine;
using VContainer;
using VContainer.Unity;


public class PlayerLifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder);

        //builder.RegisterComponentInHierarchy<PlayerHealth>().UnderTransform(transform).AsImplementedInterfaces()
          //  .AsSelf();

        builder.RegisterComponentInHierarchy<ProjectileShooter>().UnderTransform(transform);
        builder.Register<InputReader>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.RegisterComponentInHierarchy<PlayerController>().UnderTransform(transform);
        builder.RegisterComponentInHierarchy<AnimParamHandler>().UnderTransform(transform);
        builder.RegisterComponentInHierarchy<Player>().UnderTransform(transform);

        builder.Register<PlayerHealthManager>(Lifetime.Scoped).AsImplementedInterfaces();
        builder.RegisterComponentInHierarchy<IDamageable>().UnderTransform(transform).AsImplementedInterfaces();
    }
}