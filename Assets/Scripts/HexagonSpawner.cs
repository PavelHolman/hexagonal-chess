using UnityEngine;
using System.Collections.Generic;

public class HexagonSpawner : MonoBehaviour
{
    public static HexagonSpawner Instance { get; set; }

    public List<GameObject> hexagonsPrefab;
    // For storing all hexagons in the same order as they are layed out in the scene
    public GameObject[,] HexagonArray = new GameObject[11, 11];

    private void Start()
    {
        Instance = this;

        int x = 5;
        int y = 0;
        int i = 0;

        // Instantiate all hexagons on chessboard row by row with appropriate offsets
        while ((x + i) != 11)
        {
            GameObject hexagon = Instantiate(hexagonsPrefab[i % 3], GetPosition((x + i) % 11, y), Quaternion.Euler(-90f, 0f, 90f));
            HexagonArray[(x + i) % 11, y] = hexagon;
            i++;
        }

        x--;
        y++;

        while ((x + i) != 17)
        {
            GameObject hexagon = Instantiate(hexagonsPrefab[(i + 1) % 3], GetPosition((x + i - 6) % 11, y), Quaternion.Euler(-90f, 0f, 90f));
            HexagonArray[(x + i - 6) % 11, y] = hexagon;
            i++;
        }

        x--;
        y++;

        while ((x + i) != 24)
        {
            GameObject hexagon = Instantiate(hexagonsPrefab[(i + 1) % 3], GetPosition((x + i - 2) % 11, y), Quaternion.Euler(-90f, 0f, 90f));
            HexagonArray[(x + i - 2) % 11, y] = hexagon;
            i++;
        }

        x--;
        y++;

        while ((x + i) != 32)
        {
            GameObject hexagon = Instantiate(hexagonsPrefab[i % 3], GetPosition((x + i - 10) % 11, y), Quaternion.Euler(-90f, 0f, 90f));
            HexagonArray[(x + i - 10) % 11, y] = hexagon;
            i++;
        }

        x--;
        y++;

        while ((x + i) != 41)
        {
            GameObject hexagon = Instantiate(hexagonsPrefab[(i + 1) % 3], GetPosition((x + i - 8) % 11, y), Quaternion.Euler(-90f, 0f, 90f));
            HexagonArray[(x + i - 8) % 11, y] = hexagon;
            i++;
        }

        x--;
        y++;

        while ((x + i) != 51)
        {
            GameObject hexagon = Instantiate(hexagonsPrefab[(i + 1) % 3], GetPosition((x + i - 7) % 11, y), Quaternion.Euler(-90f, 0f, 90f));
            HexagonArray[(x + i - 7) % 11, y] = hexagon;
            i++;
        }

        y++;

        while ((x + i) != 61)
        {
            GameObject hexagon = Instantiate(hexagonsPrefab[(i + 1) % 3], GetPosition((x + i - 7) % 11, y), Quaternion.Euler(-90f, 0f, 90f));
            HexagonArray[(x + i - 7) % 11, y] = hexagon;
            i++;
        }

        y++;

        while ((x + i) != 70)
        {
            GameObject hexagon = Instantiate(hexagonsPrefab[(i - 1) % 3], GetPosition((x + i - 6) % 11, y), Quaternion.Euler(-90f, 0f, 90f));
            HexagonArray[(x + i - 6) % 11, y] = hexagon;
            i++;
        }

        y++;

        while ((x + i) != 78)
        {
            GameObject hexagon = Instantiate(hexagonsPrefab[(i + 1) % 3], GetPosition((x + i - 4) % 11, y), Quaternion.Euler(-90f, 0f, 90f));
            HexagonArray[(x + i - 4) % 11, y] = hexagon;
            i++;
        }

        y++;

        while ((x + i) != 85)
        {
            GameObject hexagon = Instantiate(hexagonsPrefab[(i + 1) % 3], GetPosition((x + i - 1) % 11, y), Quaternion.Euler(-90f, 0f, 90f));
            HexagonArray[(x + i - 1) % 11, y] = hexagon;
            i++;
        }

        y++;

        while ((x + i) != 91)
        {
            GameObject hexagon = Instantiate(hexagonsPrefab[(i - 1) % 3], GetPosition((x + i - 8) % 11, y), Quaternion.Euler(-90f, 0f, 90f));
            HexagonArray[(x + i - 8) % 11, y] = hexagon;
            i++;
        }
    }

    /// <summary>
    /// Get the position in scene from array coordinates
    /// </summary>
    private Vector3 GetPosition(int x, int y)
    {
        return BoardScript.Instance.VectorX * (5 - x) + BoardScript.Instance.VectorY * (5 - y) + BoardScript.Instance.VectorY * 4;
    }
}
