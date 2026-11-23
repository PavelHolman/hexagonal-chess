using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Rearranges the UI for portrait screens (phones): header on top, piece details along the
/// bottom, a single-column rules page, and a camera fitted to the space in between.
/// Also swaps mouse wording for touch wording on mobile.
/// </summary>
public class ResponsiveLayout : MonoBehaviour
{
    private struct RectState
    {
        public Vector2 AnchorMin, AnchorMax, Pivot, Position, Size;

        public static RectState Of(RectTransform rt) => new RectState
        {
            AnchorMin = rt.anchorMin, AnchorMax = rt.anchorMax, Pivot = rt.pivot,
            Position = rt.anchoredPosition, Size = rt.sizeDelta,
        };

        public void ApplyTo(RectTransform rt)
        {
            rt.anchorMin = AnchorMin;
            rt.anchorMax = AnchorMax;
            rt.pivot = Pivot;
            rt.anchoredPosition = Position;
            rt.sizeDelta = Size;
        }
    }

    private const float PortraitWidth = 720f;

    private RectTransform canvas;
    private CanvasScaler scaler;
    private Vector2 landscapeReference;
    private float landscapeMatch;
    private CameraFit cameraFit;

    private RectTransform title, turn, help, status, details, rulesButton;
    private RectTransform overlayTitle, overlayIntro, classic, special;
    private TMP_Text introText, classicText, specialText;
    private readonly Dictionary<RectTransform, RectState> landscape = new Dictionary<RectTransform, RectState>();
    private readonly Dictionary<TMP_Text, float> landscapeFontSizes = new Dictionary<TMP_Text, float>();

    private bool initialized;
    private bool? portrait;
    private Vector2Int laidOutFor;

    private void LateUpdate()
    {
        if (!initialized && !TryInitialize())
        {
            return;
        }

        var size = new Vector2Int(Screen.width, Screen.height);
        if (size == laidOutFor)
        {
            return;
        }
        laidOutFor = size;

        bool isPortrait = Screen.width < Screen.height * 0.9f;
        if (isPortrait != portrait)
        {
            portrait = isPortrait;
            if (isPortrait) ApplyPortrait(); else ApplyLandscape();
        }

        if (isPortrait)
        {
            // Rules page height depends on the screen height, so redo it on every resize
            Canvas.ForceUpdateCanvases();
            LayoutPortraitRules();
        }
    }

    /// <summary>
    /// Waits until PieceGuide has built its UI
    /// </summary>
    private bool TryInitialize()
    {
        var canvasComponent = FindAnyObjectByType<Canvas>();
        if (canvasComponent == null)
        {
            return false;
        }
        canvas = (RectTransform)canvasComponent.transform;

        details = canvas.Find("Piece Details") as RectTransform;
        rulesButton = canvas.Find("Rules Button") as RectTransform;
        var overlay = canvas.Find("Rules Overlay");
        if (details == null || rulesButton == null || overlay == null)
        {
            return false;
        }

        scaler = canvas.GetComponent<CanvasScaler>();
        landscapeReference = scaler.referenceResolution;
        landscapeMatch = scaler.matchWidthOrHeight;

        title = (RectTransform)canvas.Find("Title");
        turn = (RectTransform)canvas.Find("Turn");
        help = (RectTransform)canvas.Find("Help");
        status = (RectTransform)canvas.Find("Status");
        overlayTitle = (RectTransform)overlay.Find("Title");
        overlayIntro = (RectTransform)overlay.Find("Intro");
        classic = (RectTransform)overlay.Find("Classic");
        special = (RectTransform)overlay.Find("Special");
        introText = overlayIntro.GetComponent<TMP_Text>();
        classicText = classic.GetComponent<TMP_Text>();
        specialText = special.GetComponent<TMP_Text>();

        foreach (var rt in new[] { title, turn, help, status, details, rulesButton, overlayTitle, overlayIntro, classic, special })
        {
            landscape[rt] = RectState.Of(rt);
        }
        foreach (var t in new[] { introText, classicText, specialText })
        {
            landscapeFontSizes[t] = t.fontSize;
        }

        if (Application.isMobilePlatform)
        {
            help.GetComponent<TMP_Text>().text = "Tap a piece to see what it does and where it can go";
            rulesButton.Find("Label").GetComponent<TMP_Text>().text = "Rules";
            introText.text = introText.text.Replace("Click a piece", "Tap a piece").Replace("click a highlighted", "tap a highlighted");
            var close = overlay.Find("Close").GetComponent<TMP_Text>();
            close.text = close.text.Replace("Click anywhere or press H to close", "Tap anywhere to close");
        }

        cameraFit = Camera.main.gameObject.AddComponent<CameraFit>();
        initialized = true;
        return true;
    }

    private void ApplyLandscape()
    {
        scaler.referenceResolution = landscapeReference;
        scaler.matchWidthOrHeight = landscapeMatch;
        foreach (var pair in landscape) pair.Value.ApplyTo(pair.Key);
        foreach (var pair in landscapeFontSizes) pair.Key.fontSize = pair.Value;
        cameraFit.SetSafeArea(new Rect(0.03f, 0.03f, 0.94f, 0.94f));
    }

    private void ApplyPortrait()
    {
        scaler.referenceResolution = new Vector2(PortraitWidth, 1280f);
        scaler.matchWidthOrHeight = 0f;

        StretchTop(title, -26f, 24f, 190f);
        StretchTop(turn, -58f, 24f, 24f);
        StretchTop(help, -110f, 24f, 24f);
        StretchTop(status, -146f, 24f, 24f);

        // Piece details along the bottom edge; ContentSizeFitter keeps the height
        details.anchorMin = new Vector2(0, 0);
        details.anchorMax = new Vector2(1, 0);
        details.pivot = new Vector2(0.5f, 0);
        details.anchoredPosition = new Vector2(0, 20f);
        details.sizeDelta = new Vector2(-40f, details.sizeDelta.y);

        rulesButton.anchorMin = rulesButton.anchorMax = rulesButton.pivot = new Vector2(1, 1);
        rulesButton.anchoredPosition = new Vector2(-20f, -20f);
        rulesButton.sizeDelta = new Vector2(150f, 54f);

        // Board between the header (~14%) and the details panel (~26%)
        cameraFit.SetSafeArea(new Rect(0.02f, 0.27f, 0.96f, 0.58f));
    }

    /// <summary>
    /// Stacks the rules in one column and shrinks the text until it fits the screen
    /// </summary>
    private void LayoutPortraitRules()
    {
        float width = PortraitWidth - 60f;
        float available = canvas.rect.height - 90f;

        for (float factor = 1f; factor >= 0.6f; factor -= 0.05f)
        {
            foreach (var pair in landscapeFontSizes) pair.Key.fontSize = pair.Value * factor;

            float y = -40f;
            StretchTop(overlayTitle, y, 30f, 30f, 60f);
            y -= 70f;
            // TMP under-measures the centred, wrapped intro by about a line, so pad it
            y = Stack(overlayIntro, introText, y, width) - introText.fontSize * 1.4f - 16f;
            y = Stack(classic, classicText, y, width);
            y = Stack(special, specialText, y, width);

            if (-y <= available)
            {
                break;
            }
        }
    }

    private float Stack(RectTransform rt, TMP_Text text, float y, float width)
    {
        // Lay out at the final width first, then measure the wrapped text
        StretchTop(rt, y, 30f, 30f, 10f);
        text.ForceMeshUpdate();
        float height = text.preferredHeight + 12f;
        StretchTop(rt, y, 30f, 30f, height);
        return y - height;
    }

    private static void StretchTop(RectTransform rt, float y, float left, float right, float height = -1f)
    {
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(0, 1);
        rt.anchoredPosition = new Vector2(left, y);
        rt.sizeDelta = new Vector2(-(left + right), height >= 0f ? height : rt.sizeDelta.y);
    }
}
