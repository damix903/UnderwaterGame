using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;


public class GameLifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder);

        var options = builder.RegisterMessagePipe();
        builder.RegisterMessageBroker<EventPublisher, HealthChangeEventArgs>(options);
        builder.RegisterMessageBroker<DeathEvent>(options);

        builder.RegisterComponentInHierarchy<UI>();
        builder.RegisterComponentInHierarchy<PlayerStatsManager>();

        builder.RegisterComponentOnNewGameObject<ProjectileSpawnManager>(Lifetime.Singleton, "Proj").UnderTransform(transform);
        builder.RegisterComponentOnNewGameObject<ObjectPoolManager>(Lifetime.Singleton, "Pool").UnderTransform(transform);
        builder.Register<PoolableEntityFactory<ProjectileBase>>(Lifetime.Singleton).As<IEntityFactory<ProjectileBase>>();
    }
}

public enum EventPublisher {Player, Enemy, System}