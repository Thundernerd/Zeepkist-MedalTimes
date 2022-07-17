using HarmonyLib;
using TNRD.Zeepkist.MedalTimes.Events;
using TNRD.Zeepkist.MedalTimes.EventSystem;

namespace TNRD.Zeepkist.MedalTimes.Patches
{
    [HarmonyPatch(typeof(GUI_ChatGUI), nameof(GUI_ChatGUI.EnableSmallBox))]
    public class GUI_ChatGUI_EnableSmallBox
    {
        private static void Postfix()
        {
            EventDispatcher.Dispatch<EnableSmallBoxEvent>();
        }
    }
}
