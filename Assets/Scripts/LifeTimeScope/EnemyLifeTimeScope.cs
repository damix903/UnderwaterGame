using UnityEngine;
using VContainer;
using VContainer.Unity;


public class EnemyLifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder);

        builder.RegisterComponentInHierarchy<EntityHealth>().UnderTransform(transform).AsImplementedInterfaces()
            .AsSelf();
    }
}