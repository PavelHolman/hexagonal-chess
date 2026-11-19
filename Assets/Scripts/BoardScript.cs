using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class BoardScript : MonoBehaviour
{
    public static BoardScript Instance { get; set; }

    /// <summary>
    /// Coordinates of possible En passant move
    /// </summary>
    public int[] EnPassantMove { get; set; }

    /// <summary>
    /// Prefabs of all chess pieces
    /// </summary>
    public List<GameObject> ChessPiecePrefab;

    /// <summary>
    /// Chessboard array
    /// </summary>
    public ChessPiece[,] ChessPieces;

    /// <summary>
    /// Vectors X and Y in direction from bottom right to top left
    /// </summary>
    public Vector3 VectorX = new Vector3(-TILE_SHIFT, 0, TILE_HEIGTH / 2);
    public Vector3 VectorY = new Vector3(0, 0, TILE_HEIGTH);

    public TMP_Text StatusText;

    /// <summary>
    /// Array coordinates of the tile under the cursor, -1 when off the board
    /// </summary>
    public int HoverX => coordinateX;
    public int HoverY => coordinateY;

    /// <summary>
    /// Currently selected piece, null when nothing is selected
    /// </summary>
    public ChessPiece SelectedPiece => selectedChessPiece;

    /// <summary>
    /// Set while an overlay (the rules) covers the board
    /// </summary>
    public static bool InputBlocked;

    /// <summary>
    /// Array of allowed moves
    /// </summary>
    private bool[,] AllowedMoves { get; set; }

    private ChessPiece selectedChessPiece;

    private GameObject highlight;

    private List<GameObject> activeChessPieces;

    private int coordinateX = -1;
    private int coordinateY = -1;
    const float TILE_HEIGTH = 0.86602540378f;
    const float TILE_SHIFT = 0.75f;

    public bool isWhiteTurn = true;

    private void Start()
    {
        Instance = this;
        SpawnAllPieces();
    }

    private void Update()
    {
        SelectOrMoveChessPiece();
    }

    /// <summary>
    /// Select chess piece from array or move if already selected
    /// </summary>
    private void SelectOrMoveChessPiece()
    {
        AssignCoordinates();

        bool overUi = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        if (Input.GetMouseButtonDown(0) && !InputBlocked && !overUi && coordinateX > -1 && coordinateY > -1)
        {
            if (selectedChessPiece == null)
            {
                AllowedMoves = new bool[11, 11];
                SelectChessPiece(coordinateX, coordinateY);
            }
            else
            {
                MoveChessPiece(coordinateX, coordinateY);
            }
        }
    }

    /// <summary>
    /// Select a chess piece, check for the right color or if the piece is stunned, highlight possible moves
    /// </summary>
    private void SelectChessPiece(int x, int y)
    {
        if (ChessPieces[x, y] == null)
        {
            return;
        }

        if (ChessPieces[x, y].isWhite != isWhiteTurn)
        {
            StatusText.text = "Other player's turn!";
            return;
        }

        if (ChessPieces[x, y].isStunned)
        {
            StatusText.text = "The piece is stunned for one round!";
            return;
        }

        StatusText.text = "";

        // Highlight selected piece
        HighlightSelected(x, y);

        // Boolean array of possible moves
        AllowedMoves = ChessPieces[x, y].PossibleMove();

        selectedChessPiece = ChessPieces[x, y];
        Highlitghts.Instance.HighlightAllowedMoves(AllowedMoves);

        if ((selectedChessPiece.GetType() == typeof(Wizard) || selectedChessPiece.GetType() == typeof(Sniper)) && selectedChessPiece.coolDown > 0)
        {
            StatusText.text = selectedChessPiece.GetType().ToString() + " is recharging: ready in " + selectedChessPiece.coolDown + " moves.";
        }
    }

    /// <summary>
    /// Show highlight under a selected piece
    /// </summary>
    private void HighlightSelected(int x, int y)
    {
        if (highlight == null)
        {
            highlight = Instantiate(Highlitghts.Instance.highlightPrefab);
        }
        highlight.SetActive(true);
        highlight.transform.position = (BoardScript.Instance.VectorX * (5 - x)) + (BoardScript.Instance.VectorY * (5 - y)) + (Vector3.up * 0.07f);
    }

    /// <summary>
    /// Move the selected piece if possible
    /// </summary>
    private void MoveChessPiece(int x, int y)
    {
        // If selected move is allowed, perform the move
        if (AllowedMoves[x, y])
        {
            int score = selectedChessPiece.score;

            // Perform capture if possible, return out if game ended
            if (selectedChessPiece.GetType() != typeof(Wizard))
            {
                Capture(x, y, out bool end);
                if (end)
                {
                    return;
                }
            }

            // Perform En Passant capture if possible
            EnPassantCapture(x, y);

            // Pawn promotes into Queen on last row, En Passant move is allowed after Pawn's double step
            PromotionEnPassantCheck(x, y);

            // Pawn promotes into Queen for 3 captures
            if (selectedChessPiece.GetType() == typeof(Pawn) && selectedChessPiece.score == 3)
            {
                Promotion(x, y);
                selectedChessPiece.score = 3;
            }

            // Move a piece
            Move(x, y);


            // Give assassin one more move if he did not capture yet
            if (selectedChessPiece.GetType() == typeof(Assassin) && score == selectedChessPiece.score && !selectedChessPiece.doubleMove)
            {
                selectedChessPiece.doubleMove = true;
                Highlitghts.Instance.HideHighlights();
                highlight.SetActive(false);
                SelectChessPiece(selectedChessPiece.CurrentX, selectedChessPiece.CurrentY);
                return;
            }

            // Stun the target, give wizard cooldown
            else if (selectedChessPiece.GetType() == typeof(Wizard))
            {
                StunCooldown(x, y);
            }

            // Give sniper cooldown
            else if (selectedChessPiece.GetType() == typeof(Sniper))
            {
                ShotCooldown();
            }

            // Switch turn
            isWhiteTurn = !isWhiteTurn;
            selectedChessPiece.doubleMove = false;

            // Shorten cooldown each turn
            CoolDowns();

            // Remove stun after 1 turn
            RemoveStun();
        }

        // Unselect a piece unless assassin has second move
        if (!selectedChessPiece.doubleMove)
        {
            StatusText.text = "";
            Highlitghts.Instance.HideHighlights();
            highlight.SetActive(false);
            selectedChessPiece = null;
        }
    }

    /// <summary>
    /// Cooldown a stun of a Wizard, stun selected enemy
    /// </summary>
    private void StunCooldown(int x, int y)
    {
        selectedChessPiece.coolDown = 10;
        ChessPieces[x, y].isStunned = true;
    }

    /// <summary>
    /// Cooldown a shot of a Sniper
    /// </summary>
    private void ShotCooldown()
    {
        selectedChessPiece.coolDown = 10;
    }

    /// <summary>
    /// Remove all stuns of a team in enemy turn
    /// </summary>
    private void RemoveStun()
    {
        foreach (var c in ChessPieces)
        {
            if (c != null && c.isWhite == !isWhiteTurn)
            {
                c.isStunned = false;
            }
        }
    }

    /// <summary>
    /// Reduce cooldowns each turn
    /// </summary>
    private void CoolDowns()
    {
        ChessPiece c = ChessPieces[0, 5];
        c.coolDown--;
        c = ChessPieces[0, 10];
        c.coolDown--;
        c = ChessPieces[10, 0];
        c.coolDown--;
        c = ChessPieces[10, 5];
        c.coolDown--;
    }

    /// <summary>
    /// Move a piece if it can be moved
    /// </summary>
    private void Move(int x, int y)
    {
        if (selectedChessPiece.GetType() == typeof(Sniper) || selectedChessPiece.GetType() == typeof(Wizard))
        {
            return;
        }
        ChessPieces[selectedChessPiece.CurrentX, selectedChessPiece.CurrentY] = null;
        StartCoroutine(Glide(selectedChessPiece.transform, GetPosition(x, y)));
        selectedChessPiece.SetPosition(x, y);
        ChessPieces[x, y] = selectedChessPiece;
    }

    /// <summary>
    /// Slide a piece to its new tile with a small hop
    /// </summary>
    private IEnumerator Glide(Transform piece, Vector3 target)
    {
        Vector3 start = piece.position;
        float hop = Mathf.Min(0.4f, Vector3.Distance(start, target) * 0.1f);
        for (float t = 0; t < 1f; t += Time.deltaTime / 0.28f)
        {
            if (piece == null)
            {
                yield break;
            }
            float e = Mathf.SmoothStep(0f, 1f, t);
            piece.position = Vector3.Lerp(start, target, e) + Vector3.up * (Mathf.Sin(Mathf.PI * e) * hop);
            yield return null;
        }
        if (piece != null)
        {
            piece.position = target;
        }
    }

    /// <summary>
    /// Function to spawn all pieces, called when the game is started or reset
    /// </summary>
    private void SpawnAllPieces()
    {
        activeChessPieces = new List<GameObject>();
        ChessPieces = new ChessPiece[11, 11];
        EnPassantMove = new int[2] { -1, -1 };

        // White Assassin
        SpawnChessPiece(12, 5, 6);

        // White Sniper
        SpawnChessPiece(13, 10, 5);

        // White Wizard
        SpawnChessPiece(14, 0, 10);

        // White Pawns
        SpawnChessPiece(0, 4, 7);
        SpawnChessPiece(0, 3, 8);
        SpawnChessPiece(0, 2, 9);
        SpawnChessPiece(0, 1, 10);
        SpawnChessPiece(0, 6, 6);
        SpawnChessPiece(0, 7, 6);
        SpawnChessPiece(0, 8, 6);
        SpawnChessPiece(0, 9, 6);

        // White Knights
        SpawnChessPiece(1, 3, 10);
        SpawnChessPiece(1, 7, 8);

        // White Bishops
        SpawnChessPiece(2, 5, 8);
        SpawnChessPiece(2, 5, 9);
        SpawnChessPiece(2, 5, 10);

        // White Rooks
        SpawnChessPiece(3, 2, 10);
        SpawnChessPiece(3, 8, 7);

        // White Queen and King
        SpawnChessPiece(4, 4, 10);
        SpawnChessPiece(5, 6, 9);

        // Black Assassin
        SpawnChessPiece(15, 5, 4);

        // Black Sniper
        SpawnChessPiece(16, 10, 0);

        // Black Wizard
        SpawnChessPiece(17, 0, 5);

        // Black Pawns
        SpawnChessPiece(6, 4, 4);
        SpawnChessPiece(6, 3, 4);
        SpawnChessPiece(6, 2, 4);
        SpawnChessPiece(6, 1, 4);
        SpawnChessPiece(6, 6, 3);
        SpawnChessPiece(6, 7, 2);
        SpawnChessPiece(6, 8, 1);
        SpawnChessPiece(6, 9, 0);

        // Black Knights
        SpawnChessPiece(7, 3, 2);
        SpawnChessPiece(7, 7, 0);

        // Black Bishops
        SpawnChessPiece(8, 5, 0);
        SpawnChessPiece(8, 5, 1);
        SpawnChessPiece(8, 5, 2);

        // Black Rooks
        SpawnChessPiece(9, 2, 3);
        SpawnChessPiece(9, 8, 0);

        // Black Queen and King
        SpawnChessPiece(10, 4, 1);
        SpawnChessPiece(11, 6, 0);
    }

    /// <summary>
    /// Spawn chess piece with given index from prefab list on given position
    /// Rotates black knight, assassin, sniper and wizard by 180 degrees
    /// </summary>
    private void SpawnChessPiece(int index, int x, int y)
    {
        Quaternion quat;
        if (index == 7 || index == 15 || index == 16 || index == 17)
        {
            quat = new Quaternion(0, 180, 0, 0);
        }
        else
        {
            quat = Quaternion.identity;
        }

        GameObject chessPiece = Instantiate(ChessPiecePrefab[index], GetPosition(x, y), quat);
        chessPiece.layer = LayerMask.NameToLayer("Ignore Raycast");
        chessPiece.transform.SetParent(transform);
        ChessPieces[x, y] = chessPiece.GetComponent<ChessPiece>();
        ChessPieces[x, y].SetPosition(x, y);
        activeChessPieces.Add(chessPiece);
    }

    /// <summary>
    /// Get the position in scene from array coordinates
    /// </summary>
    private Vector3 GetPosition(int x, int y)
    {
        return VectorX * (5 - x) + VectorY * (5 - y) + (Vector3.up * 0.07f);
    }

    /// <summary>
    /// Capture a piece, end game if it is King
    /// </summary>
    private void Capture(int x, int y, out bool end)
    {
        end = false;
        ChessPiece capturedPiece = ChessPieces[x, y];
        if (capturedPiece != null && capturedPiece.isWhite != isWhiteTurn)
        {
            selectedChessPiece.score++;

            if (capturedPiece.GetType() == typeof(King))
            {
                WinGame();
                end = true;
                return;
            }

            activeChessPieces.Remove(capturedPiece.gameObject);
            Destroy(capturedPiece.gameObject);
        }
    }

    /// <summary>
    /// Capture a piece with en passant move
    /// </summary>
    private void EnPassantCapture(int x, int y)
    {
        ChessPiece capturedPiece;

        if (x == EnPassantMove[0] && y == EnPassantMove[1] && selectedChessPiece.GetType() == typeof(Pawn))
        {
            selectedChessPiece.score++;

            if (isWhiteTurn)
            {
                capturedPiece = ChessPieces[x, y + 1];
            }
            else
            {
                capturedPiece = ChessPieces[x, y - 1];
            }

            activeChessPieces.Remove(capturedPiece.gameObject);
            Destroy(capturedPiece.gameObject);
        }
    }

    /// <summary>
    /// Check for promotion of a Pawn and for en passant move possibility
    /// </summary>
    private void PromotionEnPassantCheck(int x, int y)
    {
        EnPassantMove[0] = -1;
        EnPassantMove[1] = -1;

        if (selectedChessPiece.GetType() == typeof(Pawn))
        {
            if (IsLastWhiteRow(x, y))
            {
                Promotion(x, y);
            }
            else if (IsLastBlackRow(x, y))
            {
                Promotion(x, y);
            }

            if (selectedChessPiece.CurrentY - 2 == y)
            {
                EnPassantMove[0] = x;
                EnPassantMove[1] = y + 1;
            }
            else if (selectedChessPiece.CurrentY + 2 == y)
            {
                EnPassantMove[0] = x;
                EnPassantMove[1] = y - 1;
            }
        }
    }

    /// <summary>
    /// Checks if a white piece is on a last row
    /// </summary>
    private bool IsLastWhiteRow(int x, int y)
    {
        return ((x == 0 && y == 5) || (x == 1 && y == 4) || (x == 2 && y == 3) || (x == 3 && y == 2) || (x == 4 && y == 1) || (y == 0));
    }

    /// <summary>
    /// Check if a black piece is on a last row
    /// </summary>
    private bool IsLastBlackRow(int x, int y)
    {
        return ((x == 10 && y == 5) || (x == 9 && y == 6) || (x == 8 && y == 7) || (x == 7 && y == 8) || (x == 6 && y == 9) || (y == 10));
    }

    /// <summary>
    /// Spawn Queen instead of Pawn
    /// </summary>
    private void Promotion(int x, int y)
    {
        activeChessPieces.Remove(selectedChessPiece.gameObject);
        Destroy(selectedChessPiece.gameObject);

        if (isWhiteTurn)
        {
            SpawnChessPiece(4, x, y);
            
        }
        else
        {
            SpawnChessPiece(10, x, y);
        }

        selectedChessPiece = ChessPieces[x, y];
    }

    /// <summary>
    /// Assign values from scene to array coordinates
    /// </summary>
    private void AssignCoordinates()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 50f, LayerMask.GetMask("Default")))
        {
            FindCoords(hit, out coordinateX, out coordinateY);
        }
        else
        {
            coordinateX = -1;
            coordinateY = -1;
        }
    }
    
    /// <summary>
    /// Find array coordinates based on targeted hexagon
    /// </summary>
    private void FindCoords(RaycastHit hit, out int x, out int y)
    {
        for (int i = 0; i < 11; i++)
        {
            for (int j = 0; j < 11; j++)
            {
                if (hit.collider.gameObject == HexagonSpawner.Instance.HexagonArray[i, j])
                {
                    x = i;
                    y = j;
                    return;
                }
            }
        }
        x = -1;
        y = -1;
    }

    /// <summary>
    /// End the game, announce winner
    /// </summary>
    private void WinGame()
    {
        if (isWhiteTurn)
        {
            StatusText.text = "White won the game!";
        }
        else
        {
            StatusText.text = "Black won the game!";
        }
        
        foreach (GameObject chessPiece in activeChessPieces)
        {
            Destroy(chessPiece);
        }
        
        Reset();
    }

    /// <summary>
    /// Reset the game
    /// </summary>
    private void Reset()
    {
        isWhiteTurn = true;
        selectedChessPiece = null;
        Highlitghts.Instance.HideHighlights();
        highlight.SetActive(false);
        SpawnAllPieces();
    }
}
