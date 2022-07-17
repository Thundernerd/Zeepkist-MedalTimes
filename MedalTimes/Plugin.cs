using System;
using System.IO;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using UniverseLib;
using UniverseLib.Config;

namespace TNRD.Zeepkist.MedalTimes
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private Harmony harmony;
        private MedalDrawer medalDrawer;

        private void Awake()
        {
            Universe.Init(1f,
                () =>
                {
                    Logger.LogInfo("Universe initialized");
                },
                (s, type) =>
                {
                    switch (type)
                    {
                        case LogType.Error:
                            Logger.LogError(s);
                            break;
                        case LogType.Assert:
                            Logger.LogInfo(s);
                            break;
                        case LogType.Warning:
                            Logger.LogWarning(s);
                            break;
                        case LogType.Log:
                            Logger.LogInfo(s);
                            break;
                        case LogType.Exception:
                            Logger.LogFatal(s);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(type), type, null);
                    }
                },
                new UniverseLibConfig()
                {
                    Unhollowed_Modules_Folder = Path.Combine(Paths.BepInExRootPath, "unhollowed")
                });

            harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            harmony.PatchAll();

            medalDrawer = gameObject.AddComponent<MedalDrawer>();

            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }

        private void OnDestroy()
        {
            harmony.UnpatchSelf();
        }
    }
}
