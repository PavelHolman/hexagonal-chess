using UnityEngine;

public class King : ChessPiece
{
    public override bool[,] PossibleMove()
    {
        bool[,] moves = new bool[11, 11];

        ChessPiece targetedPiece;
        int x;
        int y;
        int max;

        // Rook
        // Right Up
        x = CurrentX;
        y = CurrentY;
        max = x + 1;

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
        max = x + 1;

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
        max = y + 1;

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
        max = x - 1;

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
        max = x - 1;

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
        max = y - 1;

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
        max = x + 2;

        while (x < 9 && y > 0 && x < max)
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
        max = x + 1;

        while (x < 10 && y < 10 && x < max)
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
        max = y + 2;

        while (x > 0 && y < 9 && y < max)
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
        max = x - 2;

        while (x > 1 && y < 10 && x > max)
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
        max = x - 1;

        while (x > 0 && y > 0 && x > max)
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
