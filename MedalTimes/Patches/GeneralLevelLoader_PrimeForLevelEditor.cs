using HarmonyLib;
using TNRD.Zeepkist.MedalTimes.Events;
using TNRD.Zeepkist.MedalTimes.EventSystem;

namespace TNRD.Zeepkist.MedalTimes.Patches
{
    [HarmonyPatch(typeof(GeneralLevelLoader), nameof(GeneralLevelLoader.PrimeForLevelEditor))]
    public class GeneralLevelLoader_PrimeForLevelEditor
    {
        private static void Postfix(LEV_LevelEditorCentral theCentral, string[] theLines, SkyboxManager theSkybox)
        {
            EventDispatcher.Dispatch(new LevelLoadedEvent(GameMode.Editor));
        }
    }
}
