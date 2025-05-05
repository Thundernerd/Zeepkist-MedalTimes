using System;
using HarmonyLib;

namespace TNRD.Zeepkist.MedalTimes.Patches
{
    [HarmonyPatch(typeof(SetupGame), nameof(SetupGame.Awake))]
    public class SetupGame_Awake
    {
        public static event Action<SetupGame> PostfixEvent; 

        private static void Postfix(SetupGame __instance)
        {
            PostfixEvent?.Invoke(__instance);
        }
    }
}
