using System.Collections.Generic;

namespace TNRD.Zeepkist.MedalTimes.EventSystem
{
    internal class SubscriptionPool
    {
        private readonly Dictionary<Subscription, EventData> subscriptionToData =
            new Dictionary<Subscription, EventData>();

        private readonly List<EventData> eventData = new List<EventData>();

        public void Add(EventData data)
        {
            subscriptionToData.Add(data.Subscription, data);
            eventData.Add(data);
        }

        public bool Remove(Subscription subscription)
        {
            if (!subscriptionToData.TryGetValue(subscription, out EventData data))
            {
                // Uh oh
            }

            return eventData.Remove(data) && subscriptionToData.Remove(subscription);
        }

        public void Invoke(object data)
        {
            foreach (EventData instance in eventData)
            {
                instance.Invoke(data);
            }
        }
    }
}
