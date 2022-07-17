using System;
using System.Collections.Generic;

namespace TNRD.Zeepkist.MedalTimes.EventSystem
{
    internal static class EventPools
    {
        private static readonly Dictionary<Type, SubscriptionPool>
            typeToPool = new Dictionary<Type, SubscriptionPool>();

        public static void AddToPool<T>(EventData<T> eventData)
            where T : struct
        {
            if (!typeToPool.TryGetValue(typeof(T), out SubscriptionPool pool))
            {
                typeToPool.Add(typeof(T), new SubscriptionPool());
            }

            typeToPool[typeof(T)].Add(eventData);
        }

        public static bool RemoveFromPool(Subscription subscription)
        {
            if (!typeToPool.TryGetValue(subscription.Type, out SubscriptionPool pool))
            {
                // Uh oh
                return false;
            }

            return pool.Remove(subscription);
        }

        public static bool TryGetPool<T>(out SubscriptionPool subscriptionPool)
            where T : struct
        {
            return typeToPool.TryGetValue(typeof(T), out subscriptionPool);
        }
    }
}
