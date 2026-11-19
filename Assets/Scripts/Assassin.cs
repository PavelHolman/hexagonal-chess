using UnityEngine;

public class Assassin : ChessPiece
{
    public override bool[,] PossibleMove()
    {
        bool[,] moves = new bool[11, 11];

        ChessPiece targetedPiece;
        int x;
        int y;

        int max;

        // Right Up
        x = CurrentX;
        y = CurrentY;
        max = x + 3;
        while (x < 10 && y > 0 && x < max)
        {
            x++;
            y--;
            targetedPiece = BoardScript.Instance.ChessPieces[x, y];
            if (targetedPiece == null)
            {
                moves[x, y] = true;
            }
            else
            {
                if (targetedPiece.isWhite != isWhite && targetedPiece.GetType() != typeof(Sniper) && targetedPiece.GetType() != typeof(Wizard))
                {
                    moves[x, y] = true;
                }

                break;
            }
        }

        // Right Down
        x = CurrentX;
        y = CurrentY;
        max = x + 3;
        while (x < 10 && x < max)
        {
            x++;
            targetedPiece = BoardScript.Instance.ChessPieces[x, y];
            if (targetedPiece == null)
            {
                moves[x, y] = true;
            }
            else
            {
                if (targetedPiece.isWhite != isWhite && targetedPiece.GetType() != typeof(Sniper) && targetedPiece.GetType() != typeof(Wizard))
                {
                    moves[x, y] = true;
                }

                break;
            }
        }

        // Down
        x = CurrentX;
        y = CurrentY;
        max = y + 3;
        while (y < 10 && y < max)
        {
            y++;
            targetedPiece = BoardScript.Instance.ChessPieces[x, y];
            if (targetedPiece == null)
            {
                moves[x, y] = true;
            }
            else
            {
                if (targetedPiece.isWhite != isWhite && targetedPiece.GetType() != typeof(Sniper) && targetedPiece.GetType() != typeof(Wizard))
                {
                    moves[x, y] = true;
                }

                break;
            }
        }

        // Left Down
        x = CurrentX;
        y = CurrentY;
        max = x - 3;
        while (x > 0 && y < 10 && x > max)
        {
            x--;
            y++;
            targetedPiece = BoardScript.Instance.ChessPieces[x, y];
            if (targetedPiece == null)
            {
                moves[x, y] = true;
            }
            else
            {
                if (targetedPiece.isWhite != isWhite && targetedPiece.GetType() != typeof(Sniper) && targetedPiece.GetType() != typeof(Wizard))
                {
                    moves[x, y] = true;
                }

                break;
            }
        }

        // Left Up
        x = CurrentX;
        y = CurrentY;
        max = x - 3;
        while (x > 0 && x > max)
        {
            x--;
            targetedPiece = BoardScript.Instance.ChessPieces[x, y];
            if (targetedPiece == null)
            {
                moves[x, y] = true;
            }
            else
            {
                if (targetedPiece.isWhite != isWhite && targetedPiece.GetType() != typeof(Sniper) && targetedPiece.GetType() != typeof(Wizard))
                {
                    moves[x, y] = true;
                }

                break;
            }
        }

        // Up
        x = CurrentX;
        y = CurrentY;
        max = y - 3;
        while (y > 0 && y > max)
        {
            y--;
            targetedPiece = BoardScript.Instance.ChessPieces[x, y];
            if (targetedPiece == null)
            {
                moves[x, y] = true;
            }
            else
            {
                if (targetedPiece.isWhite != isWhite && targetedPiece.GetType() != typeof(Sniper) && targetedPiece.GetType() != typeof(Wizard))
                {
                    moves[x, y] = true;
                }

                break;
            }
        }

        return BoundsOfMoves(moves);
    }
}
