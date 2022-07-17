using HarmonyLib;
using TNRD.Zeepkist.MedalTimes.Events;
using TNRD.Zeepkist.MedalTimes.EventSystem;

namespace TNRD.Zeepkist.MedalTimes.Patches
{
    [HarmonyPatch(typeof(PauseMenu), nameof(PauseMenu.DoQuitAdventueMap))]
    public class PauseMenu_DoQuitAdventureMap
    {
        private static void Postfix()
        {
            EventDispatcher.Dispatch<QuitAdventureMapEvent>();
        }
    }
}
