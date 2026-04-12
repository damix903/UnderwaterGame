using UnityEngine;
using VContainer;
using VContainer.Unity;
using Manager.Upgrade;
using PlayerSystem;
using ProjectileSystem;
using UI;

namespace LifeTimeScope
{
    public class GameLifeTimeScope : LifetimeScope
    {
        [SerializeField] private GameConfigData gameConfigData;
        
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.RegisterInstance<GameConfigData>(gameConfigData).AsImplementedInterfaces();

            new EnemyInstaller().Install(builder);
            new FactoryInstaller().Install(builder);
            new MessagePipeInstaller().Install(builder);


            builder.RegisterComponentInHierarchy<UIHUD>();
            builder.RegisterEntryPoint<UpgradePresenter>();
            builder.RegisterComponentInHierarchy<UpgradeView>().AsImplementedInterfaces();
            builder.RegisterComponentInHierarchy<UpgradeManager>().AsImplementedInterfaces();
            builder.Register<RunState>(Lifetime.Singleton);

            builder.Register<PlayerProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<InputReader>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ProjectileSpawnManager>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterComponentOnNewGameObject<ObjectPoolManager>(Lifetime.Singleton).UnderTransform(transform);
            builder.RegisterComponentOnNewGameObject<EnemySpawner>(Lifetime.Singleton).UnderTransform(transform);
            builder.RegisterComponentOnNewGameObject<ItemManager>(Lifetime.Singleton).UnderTransform(transform);
            builder.RegisterComponentInHierarchy<PlayerStatsManager>();

            builder.RegisterComponentInHierarchy<StatgeGenerator>();
        }
    }
}

public enum EventPublisher {Player, Enemy, System, Others}