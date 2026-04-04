using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;


public class GameLifeTimeScope : LifetimeScope
{
    [SerializeField] private GameConfigData gameConfigData;
    
    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder);

        var options = builder.RegisterMessagePipe();
        builder.RegisterMessageBroker<EventPublisher, HealthChangeEventArgs>(options);
        builder.RegisterMessageBroker<DeathEvent>(options);
        builder.RegisterMessageBroker<ReleaseType>(options);
        builder.RegisterMessageBroker<EventPublisher, ComboEvent>(options);
        builder.RegisterMessageBroker<EventPublisher, ItemEvent>(options);

        builder.RegisterComponentInHierarchy<UI>();

        builder.RegisterComponentOnNewGameObject<ProjectileSpawnManager>(Lifetime.Singleton, "Proj").UnderTransform(transform);
        builder.RegisterComponentOnNewGameObject<ObjectPoolManager>(Lifetime.Singleton, "Pool").UnderTransform(transform);
        builder.RegisterComponentOnNewGameObject<EnemySpawner>(Lifetime.Singleton).UnderTransform(transform);
        builder.RegisterComponentOnNewGameObject<ItemManager>(Lifetime.Singleton).UnderTransform(transform);
        builder.RegisterComponentInHierarchy<PlayerStatsManager>();
        builder.Register<PoolableEntityFactory<Projectile>>(Lifetime.Singleton).As<IEntityFactory<Projectile>>();

        builder.RegisterComponentInHierarchy<StatgeGenerator>();
        builder.Register<PoolableEntityFactory<Room>>(Lifetime.Singleton).As<IEntityFactory<Room>>();
        builder.Register<PoolableEntityFactory<Enemy>>(Lifetime.Singleton).As<IEntityFactory<Enemy>>();
        builder.Register<PoolableEntityFactory<Item>>(Lifetime.Singleton).As<IEntityFactory<Item>>();
        builder.Register<Room>(Lifetime.Transient);
        builder.Register<Item>(Lifetime.Transient);

        builder.RegisterInstance(gameConfigData).AsImplementedInterfaces();
    }
}

public enum EventPublisher {Player, Enemy, System, Others}