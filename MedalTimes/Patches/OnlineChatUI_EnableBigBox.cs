using System;
using HarmonyLib;

namespace TNRD.Zeepkist.MedalTimes.Patches;

[HarmonyPatch(typeof(OnlineChatUI), nameof(OnlineChatUI.EnableBigBox))]
public class OnlineChatUI_EnableBigBox
{
    public static event Action PostfixEvent;

    private static void Postfix()
    {
        PostfixEvent?.Invoke();
    }
}
