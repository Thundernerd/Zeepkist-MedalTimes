using HarmonyLib;
using TNRD.Zeepkist.MedalTimes.Events;
using TNRD.Zeepkist.MedalTimes.EventSystem;

namespace TNRD.Zeepkist.MedalTimes.Patches
{
    [HarmonyPatch(typeof(GUI_ChatGUI), "EnableBigBox")]
    public class GUI_ChatGUI_EnableBigBox
    {
        private static void Postfix()
        {
            EventDispatcher.Dispatch<EnableBigBoxEvent>();
        }
    }
}
