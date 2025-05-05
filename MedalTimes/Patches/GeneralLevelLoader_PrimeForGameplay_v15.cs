using System;
using HarmonyLib;

namespace TNRD.Zeepkist.MedalTimes.Patches
{
    [HarmonyPatch(typeof(GeneralLevelLoader), nameof(GeneralLevelLoader.PrimeForGameplay_v15))]
    public class GeneralLevelLoader_PrimeForGameplay_v15
    {
        public static event Action PostfixEvent;
        
        private static void Postfix()
        {
            PostfixEvent?.Invoke();
        }
    }
}
