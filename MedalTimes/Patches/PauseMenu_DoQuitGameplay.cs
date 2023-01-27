using System;
using HarmonyLib;

namespace TNRD.Zeepkist.MedalTimes.Patches
{
    [HarmonyPatch(typeof(AdventurePauseUI), nameof(AdventurePauseUI.OnQuit))]
    public class PauseMenu_DoQuitGameplay
    {
        public static event Action PostfixEvent;

        private static void Postfix()
        {
            PostfixEvent?.Invoke();
        }
    }
}
