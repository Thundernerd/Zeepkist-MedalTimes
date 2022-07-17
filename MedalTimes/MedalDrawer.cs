using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using TNRD.Zeepkist.MedalTimes.Events;
using TNRD.Zeepkist.MedalTimes.EventSystem;
using UnityEngine;
using UnityEngine.TextCore;
using UnityEngine.UI;
using UniverseLib;
using Object = UnityEngine.Object;

namespace TNRD.Zeepkist.MedalTimes
{
    public class MedalDrawer : MonoBehaviour
    {
        private TMP_FontAsset cachedFont;

        private SubscriptionContainer subscriptionContainer;
        private PlayerManager playerManager;

        private LevelIndex.ZeepLevel CurrentLevel => playerManager.currentZeepLevel;

        private GameObject canvas;
        private CanvasGroup canvasGroup;

        private TextMeshProUGUI authorText;
        private TextMeshProUGUI goldText;
        private TextMeshProUGUI silverText;
        private TextMeshProUGUI bronzeText;

        private void Awake()
        {
            subscriptionContainer = new SubscriptionContainer(
                EventSubscriber.Subscribe<PlayerManagerLoadedEvent>(OnPlayerManagerLoaded),
                EventSubscriber.Subscribe<LevelLoadedEvent>(OnLevelLoaded),
                EventSubscriber.Subscribe<QuitGameplayEvent>(OnQuitGameplay),
                EventSubscriber.Subscribe<QuitAdventureMapEvent>(OnQuitAdventureMap),
                EventSubscriber.Subscribe<EnableBigBoxEvent>(OnEnableBigBox),
                EventSubscriber.Subscribe<EnableSmallBoxEvent>(OnEnableSmallBox));
        }

        private void OnDestroy()
        {
            subscriptionContainer.UnsubscribeAll();
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

            authorText = CreateText(group.gameObject, new Vector2(20, -50), size, fontSize, "Author:");
            goldText = CreateText(group.gameObject, new Vector2(20, -80), size, fontSize, "Gold:");
            silverText = CreateText(group.gameObject, new Vector2(20, -110), size, fontSize, "Silver:");
            bronzeText = CreateText(group.gameObject, new Vector2(20, -140), size, fontSize, "Bronze:");
            
            group.SetLayoutVertical();

            SetVisibility(false);
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
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);

            VerticalLayoutGroup verticalLayoutGroup = group.AddComponent<VerticalLayoutGroup>();
            verticalLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
            verticalLayoutGroup.spacing = 10;
            verticalLayoutGroup.padding = new RectOffset(20, 20, 20, 20);
            verticalLayoutGroup.childForceExpandWidth = true;
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
            tmp.fontSize = fontSize;
            tmp.alignment = TextAlignmentOptions.MidlineLeft;

            if (cachedFont == null)
            {
                Object[] fontAssets = RuntimeHelper.FindObjectsOfTypeAll<TMP_FontAsset>();
                cachedFont = fontAssets.Where(x => x.name == "Code New Roman b SDF")
                    .Cast<TMP_FontAsset>()
                    .FirstOrDefault();
            }

            tmp.font = cachedFont;

            return tmp;
        }

        private void OnLevelLoaded(LevelLoadedEvent value)
        {
            CreateUI();

            const string format = "mm\\:ss\\.fff";
            authorText.text =
                string.Concat("Author: ", TimeSpan.FromSeconds(CurrentLevel.time_author).ToString(format));
            goldText.text =
                string.Concat("Gold:   ", TimeSpan.FromSeconds(CurrentLevel.time_gold).ToString(format));
            silverText.text =
                string.Concat("Silver: ", TimeSpan.FromSeconds(CurrentLevel.time_silver).ToString(format));
            bronzeText.text =
                string.Concat("Bronze: ", TimeSpan.FromSeconds(CurrentLevel.time_bronze).ToString(format));

            SetVisibility(!playerManager.currentZeepLevel.isTestLevel);
        }

        private void SetVisibility(bool visible)
        {
            canvasGroup.alpha = visible ? 1 : 0;
        }

        private void OnPlayerManagerLoaded(PlayerManagerLoadedEvent value)
        {
            playerManager = value.PlayerManager;
        }

        private void OnQuitGameplay(QuitGameplayEvent value)
        {
            SetVisibility(false);
        }

        private void OnQuitAdventureMap(QuitAdventureMapEvent value)
        {
            SetVisibility(false);
        }

        private void OnEnableBigBox(EnableBigBoxEvent value)
        {
            SetVisibility(false);
        }

        private void OnEnableSmallBox(EnableSmallBoxEvent value)
        {
            SetVisibility(true);
        }
    }
}
