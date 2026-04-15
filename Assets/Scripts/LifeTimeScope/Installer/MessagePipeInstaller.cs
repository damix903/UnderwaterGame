using MessagePipe;
using VContainer;
using VContainer.Unity;

namespace LifeTimeScope
{
    public class MessagePipeInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            var options = builder.RegisterMessagePipe();
            builder.RegisterMessageBroker<EventPublisher, HealthChangeEvent>(options);
            builder.RegisterMessageBroker<DeathEvent>(options);
            builder.RegisterMessageBroker<ReleaseType>(options);
            builder.RegisterMessageBroker<ComboEvent>(options);
            builder.RegisterMessageBroker<EventPublisher, ItemEvent>(options);
            builder.RegisterMessageBroker<EventPublisher, DamageResult>(options);
        }
    }
}