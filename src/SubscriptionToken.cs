using System;

namespace Codefire.EventBus
{
    public class SubscriptionToken
    {
        public SubscriptionToken(Type eventType)
        {
            Id = Guid.NewGuid();
            EventType = eventType;
        }

        public Guid Id { get; }
        public Type EventType { get; }
    }
}
