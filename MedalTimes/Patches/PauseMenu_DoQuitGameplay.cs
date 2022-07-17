using HarmonyLib;
using TNRD.Zeepkist.MedalTimes.Events;
using TNRD.Zeepkist.MedalTimes.EventSystem;

namespace TNRD.Zeepkist.MedalTimes.Patches
{
    [HarmonyPatch(typeof(PauseMenu), nameof(PauseMenu.DoQuitGameplay))]
    public class PauseMenu_DoQuitGameplay
    {
        private static void Postfix()
        {
            EventDispatcher.Dispatch<QuitGameplayEvent>();
        }
    }
}
