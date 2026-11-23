using UnityEngine;

/// <summary>
/// Keeps the whole board, its labels and the tall back-row pieces inside a safe part of the
/// screen when the window is narrow or in portrait. Wide screens keep the scene's framing.
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraFit : MonoBehaviour
{
    /// <summary>
    /// Outermost points of the board, coordinate labels and back-row pieces
    /// </summary>
    private static readonly Vector3[] Points =
    {
        new Vector3(0f, 0f, -5.35f),
        new Vector3(0f, 1.05f, 4.33f),
        new Vector3(-1.2f, 0f, 5.15f), new Vector3(1.2f, 0f, 5.15f),
        new Vector3(-4.8f, 0f, -1.8f), new Vector3(4.8f, 0f, -1.8f),
        new Vector3(-4.8f, 0f, 2.7f), new Vector3(4.8f, 0f, 2.7f),
        new Vector3(-3.9f, 0f, -3.1f), new Vector3(3.9f, 0f, -3.1f),
        new Vector3(-3.75f, 1.1f, 2.2f), new Vector3(3.75f, 1.1f, 2.2f),
    };

    /// <summary>
    /// Below this aspect ratio the camera is refitted; above it the scene's own framing is used
    /// </summary>
    public float fitBelowAspect = 1.25f;

    private Camera cam;
    private Vector3 basePosition;
    private Rect safeArea = new Rect(0.03f, 0.03f, 0.94f, 0.94f);
    private Vector2Int fittedFor;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        basePosition = transform.position;
    }

    /// <summary>
    /// Viewport rectangle (0–1) the board should fit into, leaving room for the UI
    /// </summary>
    public void SetSafeArea(Rect area)
    {
        safeArea = area;
        fittedFor = Vector2Int.zero;
    }

    private void LateUpdate()
    {
        var size = new Vector2Int(Screen.width, Screen.height);
        if (size == fittedFor)
        {
            return;
        }
        fittedFor = size;

        transform.position = basePosition;
        if (cam.aspect < fitBelowAspect)
        {
            Fit();
        }
    }

    private void Fit()
    {
        float tanHalf = Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);

        for (int i = 0; i < 40; i++)
        {
            float minX = float.MaxValue, maxX = float.MinValue, minY = float.MaxValue, maxY = float.MinValue;
            foreach (var p in Points)
            {
                Vector3 v = cam.WorldToViewportPoint(p);
                minX = Mathf.Min(minX, v.x);
                maxX = Mathf.Max(maxX, v.x);
                minY = Mathf.Min(minY, v.y);
                maxY = Mathf.Max(maxY, v.y);
            }

            float scale = Mathf.Max((maxX - minX) / safeArea.width, (maxY - minY) / safeArea.height);
            float offsetX = (minX + maxX) * 0.5f - safeArea.center.x;
            float offsetY = (minY + maxY) * 0.5f - safeArea.center.y;

            // Distance along the view direction to the board plane
            float distance = -transform.position.y / transform.forward.y;
            float viewHeight = 2f * distance * tanHalf;

            transform.position += transform.right * (offsetX * viewHeight * cam.aspect)
                                + transform.up * (offsetY * viewHeight)
                                - transform.forward * (distance * (scale - 1f));
        }
    }
}
