using System;
using BepInEx.Logging;

namespace TNRD.Zeepkist.MedalTimes.EventSystem
{
    public class EventDispatcher
    {
        private static readonly ManualLogSource logger =
            new ManualLogSource($"{PluginInfo.PLUGIN_NAME}.{nameof(EventDispatcher)}");

        public static void Dispatch<T>()
            where T : struct
        {
            Dispatch(new T());
        }

        public static void Dispatch<T>(params object[] parameters)
            where T : struct
        {
            T instance = (T)Activator.CreateInstance(typeof(T), parameters);
            Dispatch<T>(instance);
        }

        public static void Dispatch<T>(T data)
            where T : struct
        {
            if (!EventPools.TryGetPool<T>(out SubscriptionPool pool))
            {
                logger.LogInfo($"No event pool found for '{typeof(T)}', no subscribers");
                return;
            }

            pool.Invoke(data);
        }
    }
}
