using System;
using BepInEx.Logging;
using TMPro;
using TNRD.Zeepkist.MedalTimes.Patches;
using UnityEngine;
using UnityEngine.UI;

namespace TNRD.Zeepkist.MedalTimes
{
    public class MedalDrawer : MonoBehaviour
    {
        private static ManualLogSource logger;

        private LevelScriptableObject CurrentLevel => PlayerManager.Instance.currentMaster.setupScript.GlobalLevel;
        private bool IsTestLevel => PlayerManager.Instance.currentMaster.setupScript.GlobalLevel.IsTestLevel;

        private GameObject canvas;
        private CanvasGroup canvasGroup;

        private TextMeshProUGUI authorText;
        private TextMeshProUGUI goldText;
        private TextMeshProUGUI silverText;
        private TextMeshProUGUI bronzeText;

        private bool canBeVisible;

        private bool IsVisible => canvasGroup.alpha > 0;

        private TMP_FontAsset fontAsset;
        private TMP_SpriteAsset spriteAsset;

        private void Awake()
        {
            logger = BepInEx.Logging.Logger.CreateLogSource("MedalTimes");

            GeneralLevelLoader_PrimeForGameplay.PostfixEvent += OnLevelLoaded;
            PauseMenu_DoQuitAdventureMap.PostfixEvent += OnQuitAdventureMap;
            PauseMenu_DoQuitGameplay.PostfixEvent += OnQuitGameplay;
            OnlineChatUI_EnableBigBox.PostfixEvent += OnEnableBigBox;
            OnlineChatUI_EnableSmallBox.PostfixEvent += OnEnableSmallBox;

            Plugin.ShowBronzeTime.SettingChanged += (sender, args) => { CreateMedalTimes(); };
            Plugin.ShowSilverTime.SettingChanged += (sender, args) => { CreateMedalTimes(); };
            Plugin.ShowGoldTime.SettingChanged += (sender, args) => { CreateMedalTimes(); };
            Plugin.ShowAuthorTime.SettingChanged += (sender, args) => { CreateMedalTimes(); };
            Plugin.AnchorPoint.SettingChanged += (sender, args) => { CreateMedalTimes(); };
            Plugin.HorizontalOffset.SettingChanged += (sender, args) => { CreateMedalTimes(); };
            Plugin.VerticalOffset.SettingChanged += (sender, args) => { CreateMedalTimes(); };

            fontAsset = Plugin.AssetBundle.LoadAsset<TMP_FontAsset>("ComicHelvetic_Heavy SDF");
            spriteAsset = Plugin.AssetBundle.LoadAsset<TMP_SpriteAsset>("medals");

            if (fontAsset == null || spriteAsset == null)
            {
                logger.LogError("Unable to load font or sprite asset");
            }
        }

        private void OnDestroy()
        {
            if (canvas != null)
            {
                Destroy(canvas);
            }

            GeneralLevelLoader_PrimeForGameplay.PostfixEvent -= OnLevelLoaded;
            PauseMenu_DoQuitAdventureMap.PostfixEvent -= OnQuitAdventureMap;
            PauseMenu_DoQuitGameplay.PostfixEvent -= OnQuitGameplay;
            OnlineChatUI_EnableBigBox.PostfixEvent -= OnEnableBigBox;
            OnlineChatUI_EnableSmallBox.PostfixEvent -= OnEnableSmallBox;
        }

        private void Update()
        {
            if (!canBeVisible)
                return;

            if (Input.GetKeyDown(Plugin.ToggleMedalTimes.Value))
            {
                SetVisibility(!IsVisible);
            }
        }

        private void CreateUI()
        {
            const int fontSize = 30;
            Vector2 size = new Vector2(500, 30);

            if (canvas != null)
            {
                Destroy(canvas);
            }

            canvas = new GameObject("MedalCanvas");
            canvas.layer = LayerMask.NameToLayer("UI");

            CreateCanvas(canvas);
            VerticalLayoutGroup group = CreateGroup(canvas);

            if (Plugin.ShowAuthorTime.Value)
                authorText = CreateText(group.gameObject, new Vector2(20, -50), size, fontSize, "<sprite index=0>");

            if (Plugin.ShowGoldTime.Value)
                goldText = CreateText(group.gameObject, new Vector2(20, -80), size, fontSize, "<sprite index=1>");

            if (Plugin.ShowSilverTime.Value)
                silverText = CreateText(group.gameObject, new Vector2(20, -110), size, fontSize, "<sprite index=2>");

            if (Plugin.ShowBronzeTime.Value)
                bronzeText = CreateText(group.gameObject, new Vector2(20, -140), size, fontSize, "<sprite index=3>");

            group.SetLayoutVertical();

            SetVisibility(Plugin.ShowMedalTimes.Value && !IsTestLevel);
        }

        private void CreateCanvas(GameObject parent)
        {
            Canvas canvas = parent.AddComponent<Canvas>();
            canvas.sortingOrder = -1;
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.None;

            CanvasScaler scaler = parent.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
            scaler.scaleFactor = 1;
            scaler.referencePixelsPerUnit = 100;

            canvasGroup = parent.AddComponent<CanvasGroup>();
        }

        private VerticalLayoutGroup CreateGroup(GameObject parent)
        {
            GameObject group = new GameObject("Group");
            group.transform.SetParent(parent.transform, false);

            RectTransform rectTransform = group.AddComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = new Vector2(Plugin.HorizontalOffset.Value, Plugin.VerticalOffset.Value);
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);

            VerticalLayoutGroup verticalLayoutGroup = group.AddComponent<VerticalLayoutGroup>();
            verticalLayoutGroup.childAlignment = (TextAnchor)Enum.Parse(typeof(TextAnchor), Plugin.AnchorPoint.Value);
            verticalLayoutGroup.spacing = 10;
            verticalLayoutGroup.padding = new RectOffset(20, 20, 20, 20);
            verticalLayoutGroup.childForceExpandWidth = false;
            verticalLayoutGroup.childForceExpandHeight = false;
            verticalLayoutGroup.childControlWidth = true;
            verticalLayoutGroup.childControlHeight = true;

            return verticalLayoutGroup;
        }

        private TextMeshProUGUI CreateText(
            GameObject parent,
            Vector2 position,
            Vector2 size,
            int fontSize,
            string content
        )
        {
            GameObject text = new GameObject("Text");
            text.transform.SetParent(parent.transform, false);

            RectTransform rectTransform = text.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(0, 1);
            rectTransform.pivot = new Vector3(0.5f, 0.5f);

            CanvasRenderer canvasRenderer = text.AddComponent<CanvasRenderer>();
            canvasRenderer.cullTransparentMesh = true;

            TextMeshProUGUI tmp = text.AddComponent<TextMeshProUGUI>();
            tmp.text = content;
            tmp.font = fontAsset;
            tmp.spriteAsset = spriteAsset;
            tmp.fontSize = fontSize;
            tmp.alignment = TextAlignmentOptions.MidlineLeft;

            rectTransform.sizeDelta = tmp.GetPreferredValues();

            return tmp;
        }

        private void OnLevelLoaded()
        {
            canBeVisible = true;
            CreateMedalTimes();
        }

        private void CreateMedalTimes()
        {
            if (!CanCreateMedalTimes())
            {
                logger.LogInfo("Cannot create medal times!");
                return;
            }

            try
            {
                CreateUI();
                UpdateText();
                SetVisibility(!IsTestLevel);
            }
            catch (Exception)
            {
                // Left empty on purpose
            }
        }

        private bool CanCreateMedalTimes()
        {
            try
            {
                bool _ = PlayerManager.Instance.currentMaster.setupScript.GlobalLevel.IsTestLevel;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void UpdateText()
        {
            const string format = "mm\\:ss\\.fff";
            authorText.text =
                string.Concat("<sprite index=0> ", TimeSpan.FromSeconds(CurrentLevel.TimeAuthor).ToString(format));
            goldText.text =
                string.Concat("<sprite index=1> ", TimeSpan.FromSeconds(CurrentLevel.TimeGold).ToString(format));
            silverText.text =
                string.Concat("<sprite index=2> ", TimeSpan.FromSeconds(CurrentLevel.TimeSilver).ToString(format));
            bronzeText.text =
                string.Concat("<sprite index=3> ", TimeSpan.FromSeconds(CurrentLevel.TimeBronze).ToString(format));
        }

        private void SetVisibility(bool visible)
        {
            if (canvasGroup == null)
                return;

            int alpha = visible && Plugin.ShowMedalTimes.Value ? 1 : 0;
            canvasGroup.alpha = alpha;
        }

        private void OnQuitGameplay()
        {
            canBeVisible = false;
            SetVisibility(false);
        }

        private void OnQuitAdventureMap()
        {
            canBeVisible = false;
            SetVisibility(false);
        }

        private void OnEnableBigBox()
        {
            canBeVisible = false;
            SetVisibility(false);
        }

        private void OnEnableSmallBox()
        {
            canBeVisible = true;
            SetVisibility(true);
        }
    }
}
