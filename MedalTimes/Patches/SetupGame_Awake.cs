using HarmonyLib;
using TNRD.Zeepkist.MedalTimes.Events;
using TNRD.Zeepkist.MedalTimes.EventSystem;

namespace TNRD.Zeepkist.MedalTimes.Patches
{
    [HarmonyPatch(typeof(SetupGame), "Awake")]
    public class SetupGame_Awake
    {
        private static void Postfix(SetupGame __instance)
        {
            EventDispatcher.Dispatch<SetupGameLoadedEvent>(__instance);
        }
    }
}
