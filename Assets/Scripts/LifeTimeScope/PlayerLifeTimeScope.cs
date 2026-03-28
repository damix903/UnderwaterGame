using UnityEngine;
using VContainer;
using VContainer.Unity;


public class PlayerLifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder);

        builder.RegisterComponentInHierarchy<PlayerHealth>().UnderTransform(transform).AsImplementedInterfaces()
            .AsSelf();
    }
}