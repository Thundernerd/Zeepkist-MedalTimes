using System;
using TMPro;
using TNRD.Zeepkist.MedalTimes.Patches;
using UnityEngine;
using UnityEngine.UI;

namespace TNRD.Zeepkist.MedalTimes
{
    public class MedalDrawer : MonoBehaviour
    {
        private LevelScriptableObject CurrentLevel => PlayerManager.Instance.currentMaster.setupScript.GlobalLevel;

        private GameObject canvas;
        private CanvasGroup canvasGroup;

        private TextMeshProUGUI authorText;
        private TextMeshProUGUI goldText;
        private TextMeshProUGUI silverText;
        private TextMeshProUGUI bronzeText;

        private void Awake()
        {
            GeneralLevelLoader_PrimeForGameplay.PostfixEvent += OnLevelLoaded;
            PauseMenu_DoQuitAdventureMap.PostfixEvent += OnQuitAdventureMap;
            PauseMenu_DoQuitGameplay.PostfixEvent += OnQuitGameplay;
            OnlineChatUI_EnableBigBox.PostfixEvent += OnEnableBigBox;
            OnlineChatUI_EnableSmallBox.PostfixEvent += OnEnableSmallBox;
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

            rectTransform.sizeDelta = tmp.GetPreferredValues();

            return tmp;
        }

        private void OnLevelLoaded()
        {
            try
            {
                CreateUI();

                const string format = "mm\\:ss\\.fff";
                authorText.text =
                    string.Concat("Author: ", TimeSpan.FromSeconds(CurrentLevel.TimeAuthor).ToString(format));
                goldText.text =
                    string.Concat("Gold:   ", TimeSpan.FromSeconds(CurrentLevel.TimeGold).ToString(format));
                silverText.text =
                    string.Concat("Silver: ", TimeSpan.FromSeconds(CurrentLevel.TimeSilver).ToString(format));
                bronzeText.text =
                    string.Concat("Bronze: ", TimeSpan.FromSeconds(CurrentLevel.TimeBronze).ToString(format));

                SetVisibility(!PlayerManager.Instance.currentMaster.setupScript.GlobalLevel.IsTestLevel);
            }
            catch (Exception e)
            {
                Plugin.Log(StackTraceUtility.ExtractStringFromException(e));
            }
        }

        private void SetVisibility(bool visible)
        {
            canvasGroup.alpha = visible ? 1 : 0;
        }

        private void OnQuitGameplay()
        {
            SetVisibility(false);
        }

        private void OnQuitAdventureMap()
        {
            SetVisibility(false);
        }

        private void OnEnableBigBox()
        {
            SetVisibility(false);
        }

        private void OnEnableSmallBox()
        {
            SetVisibility(true);
        }
    }
}
