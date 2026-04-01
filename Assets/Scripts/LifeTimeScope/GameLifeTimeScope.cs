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
        builder.RegisterMessageBroker<ReleaseType>(options);

        builder.RegisterComponentInHierarchy<UI>();
        builder.RegisterComponentInHierarchy<PlayerStatsManager>();

        builder.RegisterComponentOnNewGameObject<ProjectileSpawnManager>(Lifetime.Singleton, "Proj").UnderTransform(transform);
        builder.RegisterComponentOnNewGameObject<ObjectPoolManager>(Lifetime.Singleton, "Pool").UnderTransform(transform);
        builder.Register<PoolableEntityFactory<ProjectileBase>>(Lifetime.Singleton).As<IEntityFactory<ProjectileBase>>();

        builder.RegisterComponentInHierarchy<StatgeGenerator>();
        builder.Register<PoolableEntityFactory<Room>>(Lifetime.Singleton).As<IEntityFactory<Room>>();
        builder.Register<PoolableEntityFactory<Enemy>>(Lifetime.Singleton).As<IEntityFactory<Enemy>>();
        builder.Register<Room>(Lifetime.Transient);
    }
}

public enum EventPublisher {Player, Enemy, System}