using System;
using HarmonyLib;

namespace TNRD.Zeepkist.MedalTimes.Patches
{
    [HarmonyPatch(typeof(PauseMenuUI), nameof(PauseMenuUI.OnQuit))]
    public class PauseMenu_DoQuitAdventureMap
    {
        public static event Action PostfixEvent;
        
        private static void Postfix()
        {
            PostfixEvent?.Invoke();
        }
    }
}
