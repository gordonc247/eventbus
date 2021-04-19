using System;
using System.Collections.Generic;
using System.Linq;

namespace Codefire.EventBus.Memory
{
    public class MemorySubscriptionManager : ISubscriptionManager
    {
        private static readonly object SubscriptionsLock = new object();
        private readonly Dictionary<Type, List<SubscriptionInfo>> _subscriptions;

        public MemorySubscriptionManager()
        {
            _subscriptions = new Dictionary<Type, List<SubscriptionInfo>>();
        }

        public SubscriptionToken AddSubscription<TEvent, THandler>()
            where TEvent : IntegrationEvent
            where THandler : IIntegrationEventHandler<TEvent>
        {
            lock (SubscriptionsLock)
            {
                var eventType = typeof(TEvent);
                var handlerType = typeof(THandler);

                if (!_subscriptions.TryGetValue(eventType, out var allSubscriptions))
                {
                    allSubscriptions = new List<SubscriptionInfo>();
                    _subscriptions.Add(eventType, allSubscriptions);
                }

                var token = new SubscriptionToken(typeof(TEvent));
                allSubscriptions.Add(new SubscriptionInfo(token, handlerType));

                return token;
            }
        }

        public void RemoveSubscription(SubscriptionToken token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            lock (SubscriptionsLock)
            {
                if (!_subscriptions.TryGetValue(token.EventType, out var allSubscriptions)) return;

                allSubscriptions.RemoveAll(x => x.Token.Id == token.Id);
            }
        }

        public IEnumerable<Type> GetHandlers<TEvent>() where TEvent : IntegrationEvent
        {
            var eventType = typeof(TEvent);
            lock (SubscriptionsLock)
            {
                return _subscriptions.TryGetValue(eventType, out var allSubscriptions)
                    ? allSubscriptions.Select(x => x.Handler)
                    : Enumerable.Empty<Type>();
            }
        }
    }
}
