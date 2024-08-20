using System;
using System.IO;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using ZeepSDK.ChatCommands;

namespace TNRD.Zeepkist.MedalTimes
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private Harmony harmony;

        public static ConfigEntry<bool> ShowMedalTimes { get; private set; }
        public static ConfigEntry<bool> ShowAuthorTime { get; private set; }
        public static ConfigEntry<bool> ShowGoldTime { get; private set; }
        public static ConfigEntry<bool> ShowSilverTime { get; private set; }
        public static ConfigEntry<bool> ShowBronzeTime { get; private set; }
        public static ConfigEntry<KeyCode> ToggleMedalTimes { get; private set; }
        public static ConfigEntry<int> HorizontalOffset { get; private set; }
        public static ConfigEntry<int> VerticalOffset { get; private set; }
        public static ConfigEntry<string> AnchorPoint { get; private set; }

        public static AssetBundle AssetBundle { get; private set; }

        private void Awake()
        {
            harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            harmony.PatchAll();

            ShowMedalTimes = Config.Bind("General", "Show Medal Times", true, "Show the medal times UI");

            ShowAuthorTime = Config.Bind("UI", "Show Author Time", true, "Show the author time");
            ShowGoldTime = Config.Bind("UI", "Show Gold Time", true, "Show the gold time");
            ShowSilverTime = Config.Bind("UI", "Show Silver Time", true, "Show the silver time");
            ShowBronzeTime = Config.Bind("UI", "Show Bronze Time", true, "Show the bronze time");

            HorizontalOffset = Config.Bind("Positioning",
                "Horizontal Offset",
                0,
                "Horizontal offset of the medal times UI");
            VerticalOffset = Config.Bind("Positioning",
                "Vertical Offset",
                0,
                "Vertical offset of the medal times UI");
            AnchorPoint = Config.Bind("Positioning",
                "Anchor Point",
                "MiddleLeft",
                new ConfigDescription("Anchor point of the medal times UI",
                    new AcceptableValueList<string>(Enum.GetNames(typeof(TextAnchor)))));

            ToggleMedalTimes = Config.Bind("Keybinds", "Toggle Medal Times", KeyCode.PageUp, "Toggle visibility");

            if (!LoadAssetBundle())
            {
                Logger.LogFatal("Unable to load asset bundle");
            }

            gameObject.AddComponent<MedalDrawer>();

            ChatCommandApi.RegisterLocalChatCommand<AuthorTimeChatCommand>();

            // Plugin startup logic
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        }

        private bool LoadAssetBundle()
        {
            string dir = Path.GetDirectoryName(Info.Location);
            string assetBundlePath = dir + "/medaltimes-ui";
            try
            {
                AssetBundle = AssetBundle.LoadFromFile(assetBundlePath);
                return AssetBundle != null;
            }
            catch (Exception e)
            {
                Logger.LogFatal(e.Message);
                return false;
            }
        }

        private void OnDestroy()
        {
            harmony.UnpatchSelf();
        }
    }
}
