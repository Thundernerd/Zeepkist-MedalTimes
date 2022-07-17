using HarmonyLib;
using TNRD.Zeepkist.MedalTimes.Events;
using TNRD.Zeepkist.MedalTimes.EventSystem;

namespace TNRD.Zeepkist.MedalTimes.Patches
{
    [HarmonyPatch(typeof(GeneralLevelLoader), nameof(GeneralLevelLoader.PrimeForGameplay))]
    public class GeneralLevelLoader_PrimeForGameplay
    {
        private static void Postfix(GameMaster theMaster, string[] theLines, SkyboxManager theSkybox)
        {
            EventDispatcher.Dispatch(new LevelLoadedEvent(GameMode.Game));
        }
    }
}
