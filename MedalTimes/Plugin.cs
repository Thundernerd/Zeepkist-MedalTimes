using System;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace TNRD.Zeepkist.MedalTimes
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private Harmony harmony;

        private static ManualLogSource logger;

        private void Awake()
        {
            logger = Logger;

            harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            harmony.PatchAll();

            gameObject.AddComponent<MedalDrawer>();

            // Plugin startup logic
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        }

        private void OnDestroy()
        {
            harmony.UnpatchSelf();
        }

        public static void Log(object data)
        {
            if (logger == null)
                throw new NullReferenceException(nameof(logger));

            logger.LogInfo(data);
        }
    }
}
