using System;
using HarmonyLib;

namespace TNRD.Zeepkist.MedalTimes.Patches
{
    [HarmonyPatch(typeof(GeneralLevelLoader), nameof(GeneralLevelLoader.PrimeForGameplay))]
    public class GeneralLevelLoader_PrimeForGameplay
    {
        public static event Action PostfixEvent;
        
        private static void Postfix(GameMaster theMaster, string[] theLines, SkyboxManager theSkybox)
        {
            PostfixEvent?.Invoke();
        }
    }
}
