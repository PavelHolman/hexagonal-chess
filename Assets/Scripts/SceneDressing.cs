using TMPro;
using UnityEngine;

/// <summary>
/// Render quality, the turn indicator and the hover marker under the cursor.
/// </summary>
public class SceneDressing : MonoBehaviour
{
    public TMP_Text turnText;
    public GameObject hoverPrefab;
    public Color hoverColor = new Color(1f, 0.95f, 0.8f, 0.18f);

    private GameObject hover;
    private bool? shownWhiteTurn;

    private void Awake()
    {
        QualitySettings.antiAliasing = 4;
        QualitySettings.shadows = ShadowQuality.All;
        QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
        QualitySettings.shadowDistance = 30f;

        if (Application.isMobilePlatform)
        {
            // Lighter settings for phone GPUs
            QualitySettings.antiAliasing = 2;
            QualitySettings.shadowResolution = ShadowResolution.Medium;
        }
    }

    private void Start()
    {
        gameObject.AddComponent<PieceGuide>();
        gameObject.AddComponent<ResponsiveLayout>();

        // No hover on touch screens: the marker would stick to the last tap
        if (hoverPrefab != null && !Application.isMobilePlatform)
        {
            hover = Instantiate(hoverPrefab);
            hover.name = "Hover";
            hover.layer = LayerMask.NameToLayer("Ignore Raycast");
            foreach (var r in hover.GetComponentsInChildren<Renderer>())
            {
                r.material.color = hoverColor;
            }
            hover.SetActive(false);
        }
    }

    private void Update()
    {
        var board = BoardScript.Instance;
        if (board == null)
        {
            return;
        }

        if (turnText != null && shownWhiteTurn != board.isWhiteTurn)
        {
            shownWhiteTurn = board.isWhiteTurn;
            turnText.text = board.isWhiteTurn
                ? "White to move"
                : "Black to move";
        }

        if (hover != null)
        {
            bool onBoard = board.HoverX >= 0 && board.HoverY >= 0;
            hover.SetActive(onBoard);
            if (onBoard)
            {
                hover.transform.position = board.VectorX * (5 - board.HoverX) + board.VectorY * (5 - board.HoverY) + Vector3.up * 0.065f;
            }
        }
    }
}
