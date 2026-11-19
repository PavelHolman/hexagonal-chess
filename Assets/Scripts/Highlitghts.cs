using UnityEngine;
using System.Collections.Generic;

public class Highlitghts : MonoBehaviour
{
    public static Highlitghts Instance { get; set; }

    public GameObject highlightPrefab;
    private List<GameObject> highlights;
    
    private void Start()
    {
        Instance = this;
        highlights = new List<GameObject>();
    }

    /// <summary>
    /// Get the highlight objects with Ignore Raycast layer to not block Assisn Coordinates function
    /// </summary>
    private GameObject GetHighlightObject()
    {
        GameObject highlight = highlights.Find(g => !g.activeSelf);

        if (highlight == null)
        {
            highlight = Instantiate(highlightPrefab);
            highlights.Add(highlight);
        }

        return highlight;
    }

    /// <summary>
    /// Highlight possible moves of a chess piece and highlight location of selected
    /// </summary>
    public void HighlightAllowedMoves(bool[,] moves)
    {
        for (int x = 0; x < 11; x++)
        {
            for (int y = 0; y < 11; y++)
            {
                if (moves[x, y] == true)
                {
                    GameObject highlightMoves = GetHighlightObject();
                    highlightMoves.SetActive(true);
                    highlightMoves.transform.position = (BoardScript.Instance.VectorX * (5 - x)) + (BoardScript.Instance.VectorY * (5 - y)) + (Vector3.up * 0.07f);
                }
            }
        }
    }

    /// <summary>
    /// Hide all highlights
    /// </summary>
    public void HideHighlights()
    {
        foreach (GameObject highlight in highlights)
        {
            highlight.SetActive(false);
        }
    }
}
