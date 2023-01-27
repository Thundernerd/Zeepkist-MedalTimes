using System;
using HarmonyLib;

namespace TNRD.Zeepkist.MedalTimes.Patches;

[HarmonyPatch(typeof(OnlineChatUI), nameof(OnlineChatUI.EnableSmallBox))]
public class OnlineChatUI_EnableSmallBox
{
    public static event Action PostfixEvent;

    private static void Postfix()
    {
        PostfixEvent?.Invoke();
    }
}
