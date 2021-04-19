using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Codefire.EventBus.Memory
{
    public class MemoryEventBus : IEventBus
    {
        public MemoryEventBus(ISubscriptionManager subscriptionManager, IServiceProvider serviceProvider, ILogger<MemoryEventBus> logger)
        {
            SubscriptionManager = subscriptionManager;
            ServiceProvider = serviceProvider;
            Logger = logger;
        }

        private ISubscriptionManager SubscriptionManager { get; }
        private IServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }

        public async Task PublishAsync<TEvent>(TEvent evt)
            where TEvent : IntegrationEvent
        {
            var handlers = SubscriptionManager.GetHandlers<TEvent>();

            foreach (var handler in handlers)
            {
                var instance = ServiceProvider.GetService(handler);
                if (instance is IIntegrationEventHandler<TEvent> subscriber)
                {
                    try
                    {
                        await subscriber.HandleAsync(evt);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(new EventId(), ex, "EXCEPTION");
                    }
                }
            }
        }

        public SubscriptionToken Subscribe<TEvent, THandler>()
            where TEvent : IntegrationEvent
            where THandler : IIntegrationEventHandler<TEvent>
        {
            return SubscriptionManager.AddSubscription<TEvent, THandler>();
        }

        public void Unsubscribe(SubscriptionToken token)
        {
            SubscriptionManager.RemoveSubscription(token);
        }
    }
}
