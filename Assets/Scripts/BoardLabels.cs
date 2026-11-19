using TMPro;
using UnityEngine;

/// <summary>
/// Puts Glinski-style coordinates around the board: files a–l (no j) under each column,
/// ranks 1–11 along both outer edges.
/// </summary>
public class BoardLabels : MonoBehaviour
{
    private const string Files = "abcdefghikl";

    public Color color = new Color(0.93f, 0.86f, 0.72f, 0.8f);
    public float fontSize = 2.6f;
    public float height = 0.01f;

    private void Start()
    {
        for (int x = 0; x < 11; x++)
        {
            Spawn(Files[x].ToString(), Cell(x, Bottom(x) + 0.85f));
        }

        for (int rank = 1; rank <= 11; rank++)
        {
            for (int x = 0; x < 11; x++)
            {
                if (TryCell(x, rank, out int y))
                {
                    Spawn(rank.ToString(), Cell(x - Offset(rank), y));
                    break;
                }
            }

            for (int x = 10; x >= 0; x--)
            {
                if (TryCell(x, rank, out int y))
                {
                    Spawn(rank.ToString(), Cell(x + Offset(rank), y - Offset(rank)));
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Upper ranks sit behind Black's back row from the camera's view, so they go farther out
    /// </summary>
    private static float Offset(int rank) => rank <= 6 ? 1.05f : 1.5f;

    /// <summary>
    /// Array y of the lowest (rank 1) cell in column x
    /// </summary>
    private static int Bottom(int x) => Mathf.Min(10, 15 - x);

    /// <summary>
    /// Array y of the cell in column x on the given rank, if that cell is on the board
    /// </summary>
    private static bool TryCell(int x, int rank, out int y)
    {
        y = Bottom(x) - rank + 1;
        return y >= 0 && y <= 10 && x + y >= 5 && x + y <= 15;
    }

    /// <summary>
    /// Same mapping as BoardScript and Highlitghts, extended to fractional coordinates
    /// </summary>
    private static Vector3 Cell(float x, float y)
    {
        Vector3 vectorX = new Vector3(-0.75f, 0, 0.86602540378f / 2);
        Vector3 vectorY = new Vector3(0, 0, 0.86602540378f);
        return vectorX * (5 - x) + vectorY * (5 - y);
    }

    private void Spawn(string text, Vector3 position)
    {
        var label = new GameObject("Label " + text);
        label.transform.SetParent(transform, false);
        label.transform.position = position + Vector3.up * height;
        label.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        var tmp = label.AddComponent<TextMeshPro>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.color = color;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.rectTransform.sizeDelta = new Vector2(1f, 0.6f);
    }
}
