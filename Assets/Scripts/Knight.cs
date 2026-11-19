using UnityEngine;

public class Knight : ChessPiece
{
    public override bool[,] PossibleMove()
    {
        bool[,] moves = new bool[11, 11];

        // Up Up Right
        KnightMove(CurrentX + 1, CurrentY - 3, ref moves);

        // Up Right Right
        KnightMove(CurrentX + 2, CurrentY - 3, ref moves);

        // Right Right Up
        KnightMove(CurrentX + 3, CurrentY - 2, ref moves);

        // Right Right Down
        KnightMove(CurrentX + 3, CurrentY - 1, ref moves);

        // Down Right Right
        KnightMove(CurrentX + 2, CurrentY + 1, ref moves);

        // Down Down Right
        KnightMove(CurrentX + 1, CurrentY + 2, ref moves);

        // Down Down Left
        KnightMove(CurrentX - 1, CurrentY + 3, ref moves);

        // Down Left Left
        KnightMove(CurrentX - 2, CurrentY + 3, ref moves);

        // Left Left Down
        KnightMove(CurrentX - 3, CurrentY + 2, ref moves);

        // Left Left Up
        KnightMove(CurrentX - 3, CurrentY + 1, ref moves);

        // Up Left Left
        KnightMove(CurrentX - 2, CurrentY - 1, ref moves);

        // Up Up Left
        KnightMove(CurrentX - 1, CurrentY - 2, ref moves);

        return BoundsOfMoves(moves);
    }

    /// <summary>
    /// Return if a move is possible
    /// </summary>
    public void KnightMove(int x, int y, ref bool[,] r)
    {
        ChessPiece targetedPiece;

        if (-1 < x && x < 11 && -1 < y && y < 11)
        {
            targetedPiece = BoardScript.Instance.ChessPieces[x, y];
            if (targetedPiece == null)
            {
                r[x, y] = true;
            }
            else if (targetedPiece.isWhite != isWhite && targetedPiece.GetType() != typeof(Sniper) && targetedPiece.GetType() != typeof(Wizard))
            {
                r[x, y] = true;
            }
        }
    }
}
