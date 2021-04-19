using System.Threading.Tasks;

namespace Codefire.EventBus
{
    public interface IEventBus
    {
        Task PublishAsync<TEvent>(TEvent evt)
            where TEvent : IntegrationEvent;

        SubscriptionToken Subscribe<TEvent, THandler>()
            where TEvent : IntegrationEvent
            where THandler : IIntegrationEventHandler<TEvent>;

        void Unsubscribe(SubscriptionToken token);
    }
}
