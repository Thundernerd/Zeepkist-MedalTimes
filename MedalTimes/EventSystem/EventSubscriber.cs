using System;
using System.Collections.Generic;

namespace TNRD.Zeepkist.MedalTimes.EventSystem
{
    public delegate void OnEventDelegate<in T>(T value);

    public static class EventSubscriber
    {
        private static readonly Dictionary<Type, SubscriptionPool> typeToPool = new Dictionary<Type, SubscriptionPool>();

        public static Subscription Subscribe<T>(OnEventDelegate<T> callback)
            where T : struct
        {
            Subscription subscription = new Subscription(typeof(T));
            EventData<T> eventData = new EventData<T>(subscription, callback);
            EventPools.AddToPool(eventData);
            return subscription;
        }

        public static bool Unsubscribe(Subscription subscription)
        {
            return EventPools.RemoveFromPool(subscription);
        }
    }
}
