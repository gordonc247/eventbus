using System;
using System.Collections.Generic;
using System.Text;

namespace Codefire.EventBus
{
    public interface ISubscriptionManager
    {
        SubscriptionToken AddSubscription<TEvent, THandler>()
            where TEvent : IntegrationEvent
            where THandler : IIntegrationEventHandler<TEvent>;

        void RemoveSubscription(SubscriptionToken token);

        IEnumerable<Type> GetHandlers<TEvent>() where TEvent : IntegrationEvent;
    }
}
