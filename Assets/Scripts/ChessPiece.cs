using UnityEngine;

public abstract class ChessPiece : MonoBehaviour
{
    /// <summary>
    /// Current X coordinate of a piece
    /// </summary>
    public int CurrentX { get; set; }

    /// <summary>
    /// Current Y coordinate of a piece
    /// </summary>
    public int CurrentY { get; set; }

    public bool isWhite;
    public bool isStunned;
    public int score = 0;
    public int coolDown = 10;
    public bool doubleMove = false;

    /// <summary>
    /// Set array coordinates of a chess piece
    /// </summary>
    public void SetPosition(int x, int y)
    {
        CurrentX = x;
        CurrentY = y;
    }

    /// <summary>
    /// Return boolean array of possible moves of a chess piece
    /// </summary>
    public virtual bool[,] PossibleMove()
    {
        return new bool [11,11];
    }

    /// <summary>
    /// Disable moves out of bounds of Chessboard
    /// </summary>
    public bool[,] BoundsOfMoves(bool[,]moves)
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5 - i; j++)
            {
                moves[j, i] = false;
            }
        }

        for (int i = 10; i > 5; i--)
        {
            for (int j = 10; j > 15 - i; j--)
            {
                moves[j, i] = false;
            }
        }
        return moves;
    }
}
