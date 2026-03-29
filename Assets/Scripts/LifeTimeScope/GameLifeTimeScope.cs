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
    }
}

public enum EventPublisher {Player, Enemy, System}