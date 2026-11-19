using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Explains the pieces: a details panel for the hovered or selected piece,
/// and a rules overlay toggled with H or the Rules button.
/// </summary>
public class PieceGuide : MonoBehaviour
{
    private struct PieceDoc
    {
        public string Name;
        public string Kind;
        public string Rules;

        public PieceDoc(string name, string kind, string rules)
        {
            Name = name;
            Kind = kind;
            Rules = rules;
        }
    }

    private static readonly Dictionary<System.Type, PieceDoc> Docs = new Dictionary<System.Type, PieceDoc>
    {
        { typeof(King), new PieceDoc("King", "Classic piece",
            "Steps one tile in any of the 12 directions: the 6 neighbouring tiles and the 6 diagonals. If it is captured, its side loses.") },
        { typeof(Queen), new PieceDoc("Queen", "Classic piece",
            "Slides any distance along the 6 straight lines or the 6 diagonals.") },
        { typeof(Rook), new PieceDoc("Rook", "Classic piece",
            "Slides any distance along the 6 straight lines, through the edges of the tiles.") },
        { typeof(Bishop), new PieceDoc("Bishop", "Classic piece",
            "Slides any distance along the 6 diagonals, through the corners of the tiles. Each of the three bishops stays on one tile colour.") },
        { typeof(Knight), new PieceDoc("Knight", "Classic piece",
            "Jumps two tiles straight and then one to the side, leaping over anything in between.") },
        { typeof(Pawn), new PieceDoc("Pawn", "Classic piece",
            "Moves one tile forward (two on its first move) and captures one tile diagonally forward. Becomes a Queen on the far edge, or after its third capture. En passant applies.") },
        { typeof(Assassin), new PieceDoc("Assassin", "Special piece",
            "Moves up to 3 tiles in a straight line. If it moves without capturing, it gets a second move straight away. It can't capture the Sniper or the Wizard.") },
        { typeof(Sniper), new PieceDoc("Sniper", "Special piece",
            "Never moves. Shoots an enemy up to 2 tiles away in a straight line, or 1 diagonal step away, even over other pieces. Then reloads for 10 moves.") },
        { typeof(Wizard), new PieceDoc("Wizard", "Special piece",
            "Never moves. Stuns an enemy in the same range as the Sniper: the stunned piece can't move on its side's next turn. Then recharges for 10 moves.") },
    };

    private static readonly System.Type[] GuideOrder =
    {
        typeof(King), typeof(Queen), typeof(Rook), typeof(Bishop), typeof(Knight), typeof(Pawn),
        typeof(Assassin), typeof(Sniper), typeof(Wizard),
    };

    private static readonly Color Cream = new Color32(0xF3, 0xE9, 0xD2, 0xFF);
    private const string Muted = "#B8AC95";
    private const string Accent = "#FFD08C";

    private GameObject detailsPanel;
    private TextMeshProUGUI detailsText;
    private GameObject rulesOverlay;
    private ChessPiece shownPiece;
    private string shownState;

    private void Start()
    {
        var canvas = FindAnyObjectByType<Canvas>().transform;
        BuildDetailsPanel(canvas);
        BuildRulesButton(canvas);
        BuildRulesOverlay(canvas);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) || (rulesOverlay.activeSelf && Input.GetKeyDown(KeyCode.Escape)))
        {
            ToggleRules();
        }

        var board = BoardScript.Instance;
        if (board == null || board.ChessPieces == null)
        {
            return;
        }

        ChessPiece piece = board.SelectedPiece;
        if (piece == null && board.HoverX >= 0 && board.HoverY >= 0)
        {
            piece = board.ChessPieces[board.HoverX, board.HoverY];
        }

        string state = piece == null ? null : State(piece);
        if (piece == shownPiece && state == shownState)
        {
            return;
        }
        shownPiece = piece;
        shownState = state;

        detailsPanel.SetActive(piece != null);
        if (piece == null || !Docs.TryGetValue(piece.GetType(), out PieceDoc doc))
        {
            detailsPanel.SetActive(false);
            return;
        }

        string side = piece.isWhite ? "White" : "Black";
        detailsText.text =
            $"<size=38>{doc.Name}</size>\n" +
            $"<color={Muted}><size=20>{side.ToUpper()}  ·  {doc.Kind.ToUpper()}</size></color>\n\n" +
            $"<size=23>{doc.Rules}</size>" +
            (string.IsNullOrEmpty(state) ? "" : $"\n\n<color={Accent}><size=23>{state}</size></color>");
    }

    /// <summary>
    /// Live status of a piece: cooldowns, stuns, captures toward promotion
    /// </summary>
    private static string State(ChessPiece piece)
    {
        var lines = new List<string>();

        if (piece.isStunned)
        {
            lines.Add("Stunned: can't move this turn");
        }

        if (piece is Sniper || piece is Wizard)
        {
            string action = piece is Sniper ? "Shot" : "Stun";
            lines.Add(piece.coolDown > 0
                ? $"{action} ready in {piece.coolDown} move{(piece.coolDown == 1 ? "" : "s")}"
                : $"{action} ready");
        }
        else if (piece is Pawn)
        {
            lines.Add($"Captures: {piece.score} of 3");
        }
        else if (piece is Assassin && piece.doubleMove)
        {
            lines.Add("Second move: pick a tile");
        }

        return string.Join("\n", lines);
    }

    private void ToggleRules()
    {
        bool open = !rulesOverlay.activeSelf;
        rulesOverlay.SetActive(open);
        BoardScript.InputBlocked = open;
    }

    private void BuildDetailsPanel(Transform canvas)
    {
        detailsPanel = Panel(canvas, "Piece Details", new Color(0.05f, 0.04f, 0.03f, 0.55f));
        var rt = (RectTransform)detailsPanel.transform;
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(1, 1);
        rt.anchoredPosition = new Vector2(-36, -36);
        rt.sizeDelta = new Vector2(380, 0);

        var layout = detailsPanel.AddComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(26, 26, 22, 24);
        layout.childControlWidth = layout.childControlHeight = true;
        layout.childForceExpandHeight = false;
        detailsPanel.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        detailsText = Text(detailsPanel.transform, "Text", "", 23);
        detailsText.lineSpacing = 6;
        detailsPanel.SetActive(false);
    }

    private void BuildRulesButton(Transform canvas)
    {
        var go = Panel(canvas, "Rules Button", new Color(0.05f, 0.04f, 0.03f, 0.55f));
        var rt = (RectTransform)go.transform;
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0, 0);
        rt.anchoredPosition = new Vector2(36, 36);
        rt.sizeDelta = new Vector2(220, 52);
        go.GetComponent<Image>().raycastTarget = true;

        var button = go.AddComponent<Button>();
        var colors = button.colors;
        colors.highlightedColor = new Color(1.5f, 1.4f, 1.2f, 1f);
        button.colors = colors;
        button.onClick.AddListener(ToggleRules);

        var label = Text(go.transform, "Label", $"Rules  <color={Muted}>[H]</color>", 24);
        label.alignment = TextAlignmentOptions.Center;
        Stretch(label.rectTransform);
    }

    private void BuildRulesOverlay(Transform canvas)
    {
        rulesOverlay = Panel(canvas, "Rules Overlay", new Color(0.04f, 0.035f, 0.03f, 0.9f));
        Stretch((RectTransform)rulesOverlay.transform);
        rulesOverlay.GetComponent<Image>().raycastTarget = true;
        rulesOverlay.AddComponent<Button>().onClick.AddListener(ToggleRules);

        var title = Text(rulesOverlay.transform, "Title", "How to play", 44);
        Place(title.rectTransform, new Vector2(0.5f, 1), new Vector2(0, -60), new Vector2(1200, 60));
        title.alignment = TextAlignmentOptions.Top;

        var intro = Text(rulesOverlay.transform, "Intro",
            $"<color={Muted}>Hexagonal chess for two players on one screen, on a 91-tile board. White starts. " +
            "Click a piece to see its moves, then click a highlighted tile. " +
            "There is no check or checkmate: <color=#F3E9D2>capture the enemy King to win</color>.</color>", 24);
        Place(intro.rectTransform, new Vector2(0.5f, 1), new Vector2(0, -125), new Vector2(1200, 80));
        intro.alignment = TextAlignmentOptions.Top;

        var left = Text(rulesOverlay.transform, "Classic", Guide(0, 6), 20);
        Place(left.rectTransform, new Vector2(0.5f, 1), new Vector2(-310, -200), new Vector2(580, 720));
        var right = Text(rulesOverlay.transform, "Special", Guide(6, 9), 20);
        Place(right.rectTransform, new Vector2(0.5f, 1), new Vector2(310, -200), new Vector2(580, 720));

        var close = Text(rulesOverlay.transform, "Close", $"<color={Muted}>Click anywhere or press H to close</color>", 22);
        Place(close.rectTransform, new Vector2(0.5f, 0), new Vector2(0, 28), new Vector2(800, 40));
        close.alignment = TextAlignmentOptions.Bottom;

        rulesOverlay.SetActive(false);
    }

    private static string Guide(int from, int to)
    {
        var sb = new System.Text.StringBuilder();
        sb.Append($"<color={Accent}><size=20>{(from == 0 ? "CLASSIC PIECES" : "SPECIAL PIECES")}</size></color>\n\n");
        for (int i = from; i < to; i++)
        {
            var doc = Docs[GuideOrder[i]];
            sb.Append($"<size=25>{doc.Name}</size>\n<color={Muted}>{doc.Rules}</color>\n<size=10> </size>\n");
        }
        return sb.ToString();
    }

    private static GameObject Panel(Transform parent, string name, Color color)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(Image));
        go.transform.SetParent(parent, false);
        var image = go.GetComponent<Image>();
        image.color = color;
        image.raycastTarget = false;
        return go;
    }

    private static TextMeshProUGUI Text(Transform parent, string name, string text, float size)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        var t = go.AddComponent<TextMeshProUGUI>();
        t.text = text;
        t.fontSize = size;
        t.color = Cream;
        t.raycastTarget = false;
        t.alignment = TextAlignmentOptions.TopLeft;
        return t;
    }

    private static void Place(RectTransform rt, Vector2 anchor, Vector2 position, Vector2 size)
    {
        rt.anchorMin = rt.anchorMax = rt.pivot = anchor;
        rt.anchoredPosition = position;
        rt.sizeDelta = size;
    }

    private static void Stretch(RectTransform rt)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = rt.offsetMax = Vector2.zero;
    }
}
