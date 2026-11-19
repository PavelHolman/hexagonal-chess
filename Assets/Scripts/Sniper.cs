using UnityEngine;

public class Sniper : ChessPiece
{
    public override bool[,] PossibleMove()
    {
        bool[,] moves = new bool[11, 11];

        if (coolDown > 0)
        {
            Debug.Log("Sniper's shot is on " + coolDown + " round cooldown.");
            return moves;
        }
        else
        {
            ChessPiece targetedPiece;
            int x;
            int y;
            int max;

            // Rook
            // Right Up
            x = CurrentX;
            y = CurrentY;
            max = x + 2;

            while (x < 10 && y > 0 && x < max)
            {
                x++;
                y--;
                targetedPiece = BoardScript.Instance.ChessPieces[x, y];

                if (targetedPiece != null && targetedPiece.isWhite != isWhite)
                {
                    moves[x, y] = true;
                }
            }

            // Right Down
            x = CurrentX;
            y = CurrentY;
            max = x + 2;

            while (x < 10 && x < max)
            {
                x++;
                targetedPiece = BoardScript.Instance.ChessPieces[x, y];

                if (targetedPiece != null && targetedPiece.isWhite != isWhite)
                {
                    moves[x, y] = true;
                }
            }

            // Down
            x = CurrentX;
            y = CurrentY;
            max = y + 2;

            while (y < 10 && y < max)
            {
                y++;
                targetedPiece = BoardScript.Instance.ChessPieces[x, y];

                if (targetedPiece != null && targetedPiece.isWhite != isWhite)
                {
                    moves[x, y] = true;
                }
            }

            // Left Down
            x = CurrentX;
            y = CurrentY;
            max = x - 2;

            while (x > 0 && y < 10 && x > max)
            {
                x--;
                y++;
                targetedPiece = BoardScript.Instance.ChessPieces[x, y];

                if (targetedPiece != null && targetedPiece.isWhite != isWhite)
                {
                    moves[x, y] = true;
                }
            }

            // Left Up
            x = CurrentX;
            y = CurrentY;
            max = x - 2;

            while (x > 0 && x > max)
            {
                x--;
                targetedPiece = BoardScript.Instance.ChessPieces[x, y];

                if (targetedPiece != null && targetedPiece.isWhite != isWhite)
                {
                    moves[x, y] = true;
                }
            }

            // Up
            x = CurrentX;
            y = CurrentY;
            max = y - 2;

            while (y > 0 && y > max)
            {
                y--;
                targetedPiece = BoardScript.Instance.ChessPieces[x, y];

                if (targetedPiece != null && targetedPiece.isWhite != isWhite)
                {
                    moves[x, y] = true;
                }
            }

            // Bishop
            // Right Up
            x = CurrentX;
            y = CurrentY;
            max = x + 1;

            while (x < 10 && y > 1 && x < max)
            {
                x++;
                y -= 2;
                targetedPiece = BoardScript.Instance.ChessPieces[x, y];

                if (targetedPiece != null && targetedPiece.isWhite != isWhite)
                {
                    moves[x, y] = true;
                }
            }

            // Right
            x = CurrentX;
            y = CurrentY;
            max = x + 2;

            while (x < 9 && y > 0 && x < max)
            {
                x += 2;
                y--;
                targetedPiece = BoardScript.Instance.ChessPieces[x, y];

                if (targetedPiece != null && targetedPiece.isWhite != isWhite)
                {
                    moves[x, y] = true;
                }
            }

            // Right Down
            x = CurrentX;
            y = CurrentY;
            max = x + 1;

            while (x < 10 && y < 10 && x < max)
            {
                x++;
                y++;
                targetedPiece = BoardScript.Instance.ChessPieces[x, y];

                if (targetedPiece != null && targetedPiece.isWhite != isWhite)
                {
                    moves[x, y] = true;
                }
            }

            // Left Down
            x = CurrentX;
            y = CurrentY;
            max = y + 2;

            while (x > 0 && y < 9 && y < max)
            {
                x--;
                y += 2;
                targetedPiece = BoardScript.Instance.ChessPieces[x, y];

                if (targetedPiece != null && targetedPiece.isWhite != isWhite)
                {
                    moves[x, y] = true;
                }
            }

            // Left
            x = CurrentX;
            y = CurrentY;
            max = x - 2;

            while (x > 1 && y < 10 && x > max)
            {
                x -= 2;
                y++;
                targetedPiece = BoardScript.Instance.ChessPieces[x, y];

                if (targetedPiece != null && targetedPiece.isWhite != isWhite)
                {
                    moves[x, y] = true;
                }
            }

            // Left Up
            x = CurrentX;
            y = CurrentY;
            max = x - 1;

            while (x > 0 && y > 0 && x > max)
            {
                x--;
                y--;
                targetedPiece = BoardScript.Instance.ChessPieces[x, y];

                if (targetedPiece != null && targetedPiece.isWhite != isWhite)
                {
                    moves[x, y] = true;
                }
            }

            return BoundsOfMoves(moves);
        }
    }
}
