using UnityEngine;

public class Bishop : ChessPiece
{
    public override bool[,] PossibleMove()
    {
        bool[,] moves = new bool[11, 11];

        ChessPiece targetedPiece;
        int x;
        int y;

        // Right Up
        x = CurrentX;
        y = CurrentY;
        while (x < 10 && y > 1)
        {
            x++;
            y -= 2;
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

        // Right
        x = CurrentX;
        y = CurrentY;
        while (x < 9 && y > 0)
        {
            x += 2;
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
        while (x < 10 && y < 10)
        {
            x++;
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
        while (x > 0 && y < 9)
        {
            x--;
            y += 2;
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

        // Left
        x = CurrentX;
        y = CurrentY;
        while (x > 1 && y < 10)
        {
            x -= 2;
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
        while (x > 0 && y > 0)
        {
            x--;
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
