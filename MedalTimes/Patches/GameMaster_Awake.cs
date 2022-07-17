using HarmonyLib;
using TNRD.Zeepkist.MedalTimes.Events;
using TNRD.Zeepkist.MedalTimes.EventSystem;

namespace TNRD.Zeepkist.MedalTimes.Patches
{
    [HarmonyPatch(typeof(GameMaster), "Awake")]
    public class GameMaster_Awake
    {
        private static void Postfix(GameMaster __instance)
        {
            EventDispatcher.Dispatch(new GameMasterLoadedEvent(__instance));
        }
    }
}
