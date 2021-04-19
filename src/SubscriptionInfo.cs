using System;

namespace Codefire.EventBus
{
    public class SubscriptionInfo
    {
        public SubscriptionInfo(SubscriptionToken token, Type handler)
        {
            Token = token;
            Handler = handler;
        }

        public SubscriptionToken Token { get; }
        public Type Handler { get; }
    }
}
