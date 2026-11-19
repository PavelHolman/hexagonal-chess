using UnityEngine;

public class Pawn : ChessPiece
{
    public override bool[,] PossibleMove()
    {
        bool[,] moves = new bool[11, 11];
        ChessPiece targetedPiece, targetedPieceOnTwo;
        int[] enPassant = BoardScript.Instance.EnPassantMove;

        // White moves
        if (isWhite)
        {
            // Diagonal left
            if ((CurrentX != 0) || (CurrentX == 1 && CurrentY != 4) || (CurrentX == 2 && CurrentY != 3) || (CurrentX == 3 && CurrentY != 2) || (CurrentX == 4 && CurrentY != 1) || (CurrentX == 5 && CurrentY != 0))
            {
                if (enPassant[0] == CurrentX - 1 && enPassant[1] == CurrentY)
                {
                    moves[CurrentX - 1, CurrentY] = true;
                }

                targetedPiece = BoardScript.Instance.ChessPieces[CurrentX - 1, CurrentY];

                if (targetedPiece != null && !targetedPiece.isWhite && targetedPiece.GetType() != typeof(Sniper) && targetedPiece.GetType() != typeof(Wizard))
                {
                    moves[CurrentX - 1, CurrentY] = true;
                }
            }

            // Diagonal right
            if (CurrentX != 10 && CurrentY != 0)
            {
                if (enPassant[0] == CurrentX + 1 && enPassant[1] == CurrentY - 1)
                {
                    moves[CurrentX + 1, CurrentY - 1] = true;
                }

                targetedPiece = BoardScript.Instance.ChessPieces[CurrentX + 1, CurrentY - 1];

                if (targetedPiece != null && !targetedPiece.isWhite && targetedPiece.GetType() != typeof(Sniper) && targetedPiece.GetType() != typeof(Wizard))
                {
                    moves[CurrentX + 1, CurrentY - 1] = true;
                }
            }

            // Middle
            if ((CurrentX == 0 && CurrentY != 5) || (CurrentX == 1 && CurrentY != 4) || (CurrentX == 2 && CurrentY != 3) || (CurrentX == 3 && CurrentY != 2) || (CurrentX == 4 && CurrentY != 1) || (CurrentY != 0))
            {
                targetedPiece = BoardScript.Instance.ChessPieces[CurrentX, CurrentY - 1];

                if (targetedPiece == null)
                {
                    moves[CurrentX, CurrentY - 1] = true;
                }
            }

            // Middle on first move
            if (((CurrentX == 1 && CurrentY == 10) || (CurrentX == 2 && CurrentY == 9) || (CurrentX == 3 && CurrentY == 8) || (CurrentX == 4 && CurrentY == 7) || (CurrentX == 5 && CurrentY == 6) || (CurrentX == 6 && CurrentY == 6) || (CurrentX == 7 && CurrentY == 6) || (CurrentX == 8 && CurrentY == 6) || (CurrentX == 9 && CurrentY == 6)) && score == 0)
            {
                targetedPiece = BoardScript.Instance.ChessPieces[CurrentX, CurrentY - 1];
                targetedPieceOnTwo = BoardScript.Instance.ChessPieces[CurrentX, CurrentY - 2];

                if (targetedPiece == null && targetedPieceOnTwo == null)
                {
                    moves[CurrentX, CurrentY - 2] = true;
                }
            }
        }
        // Black moves
        else
        {
            // Diagonal left
            if ((CurrentX != 0 && CurrentY != 10))
            {
                if (enPassant[0] == CurrentX - 1 && enPassant[1] == CurrentY + 1)
                {
                    moves[CurrentX - 1, CurrentY + 1] = true;
                }

                targetedPiece = BoardScript.Instance.ChessPieces[CurrentX - 1, CurrentY + 1];

                if (targetedPiece != null && targetedPiece.isWhite && targetedPiece.GetType() != typeof(Sniper) && targetedPiece.GetType() != typeof(Wizard))
                {
                    moves[CurrentX - 1, CurrentY + 1] = true;
                }
            }

            // Diagonal right
            if ((CurrentX != 10) || (CurrentX == 9 && CurrentY != 6) || (CurrentX == 8 && CurrentY != 7) || (CurrentX == 7 && CurrentY != 8) || (CurrentX == 6 && CurrentY != 9) || (CurrentX == 5 && CurrentY != 10))
            {
                if (enPassant[0] == CurrentX + 1 && enPassant[1] == CurrentY)
                {
                    moves[CurrentX + 1, CurrentY] = true;
                }

                targetedPiece = BoardScript.Instance.ChessPieces[CurrentX + 1, CurrentY];

                if (targetedPiece != null && targetedPiece.isWhite && targetedPiece.GetType() != typeof(Sniper) && targetedPiece.GetType() != typeof(Wizard))
                {
                    moves[CurrentX + 1, CurrentY] = true;
                }
            }

            // Middle
            if ((CurrentX == 10 && CurrentY != 5) || (CurrentX == 9 && CurrentY != 6) || (CurrentX == 8 && CurrentY != 7) || (CurrentX == 7 && CurrentY != 8) || (CurrentX == 6 && CurrentY != 9) || (CurrentY != 10))
            {
                targetedPiece = BoardScript.Instance.ChessPieces[CurrentX, CurrentY + 1];

                if (targetedPiece == null)
                {
                    moves[CurrentX, CurrentY + 1] = true;
                }
            }

            // Middle on first move
            if (((CurrentX == 1 && CurrentY == 4) || (CurrentX == 2 && CurrentY == 4) || (CurrentX == 3 && CurrentY == 4) || (CurrentX == 4 && CurrentY == 4) || (CurrentX == 5 && CurrentY == 4) || (CurrentX == 6 && CurrentY == 3) || (CurrentX == 7 && CurrentY == 2) || (CurrentX == 8 && CurrentY == 1) || (CurrentX == 9 && CurrentY == 0)) && score == 0)
            {
                targetedPiece = BoardScript.Instance.ChessPieces[CurrentX, CurrentY + 1];
                targetedPieceOnTwo = BoardScript.Instance.ChessPieces[CurrentX, CurrentY + 2];

                if (targetedPiece == null && targetedPieceOnTwo == null)
                {
                    moves[CurrentX, CurrentY + 2] = true;
                }
            }
        }

        return BoundsOfMoves(moves);
    }
}
