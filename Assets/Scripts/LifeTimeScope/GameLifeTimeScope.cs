using Manager;
using Manager.AudioSystem;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Manager.Upgrade;
using PlayerSystem;
using ProjectileSystem;
using Stage;
using UI;
using Underwater.Utility.Timer;

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
            builder.RegisterComponentInHierarchy<IFader>();
            builder.Register<UpgradePresenter>(Lifetime.Singleton);

            builder.RegisterComponentInHierarchy<UpgradeView>().AsImplementedInterfaces();
            builder.RegisterComponentInHierarchy<UpgradeManager>().AsImplementedInterfaces().AsSelf();
            builder.Register<RunState>(Lifetime.Singleton);

            builder.RegisterComponentInHierarchy<CameraManager>();
            
            builder.Register<PlayerProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<InputReader>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ProjectileSpawnManager>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterComponentOnNewGameObject<ObjectPoolManager>(Lifetime.Singleton).UnderTransform(transform);
            builder.RegisterComponentOnNewGameObject<EnemySpawner>(Lifetime.Singleton).UnderTransform(transform);
            builder.RegisterComponentOnNewGameObject<ItemManager>(Lifetime.Singleton).UnderTransform(transform);
            builder.RegisterComponentInHierarchy<PlayerStatsManager>();
            builder.RegisterEntryPoint<RunManager>(Lifetime.Singleton);

            builder.RegisterComponentInHierarchy<StageGenerator>();
            builder.RegisterComponentInHierarchy<SoundManager>();

            var timer = new TimerManager();
            builder.RegisterInstance(timer).AsImplementedInterfaces();
            Timer.Initialize(timer);
        }
    }
}

public enum EventPublisher {Player, Enemy, System, Others}