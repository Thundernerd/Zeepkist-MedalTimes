using HarmonyLib;
using TNRD.Zeepkist.MedalTimes.Events;
using TNRD.Zeepkist.MedalTimes.EventSystem;

namespace TNRD.Zeepkist.MedalTimes.Patches
{
    [HarmonyPatch(typeof(PlayerManager), "Awake")]
    public class PlayerManager_Awake
    {
        private static void Postfix(PlayerManager __instance)
        {
            EventDispatcher.Dispatch(new PlayerManagerLoadedEvent(__instance)); 
        }
    }
}
