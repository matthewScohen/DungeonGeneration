using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Dungeon2DView : VisualElement
{
    private const float BaseTileSize = 32f;

    private Dungeon2DGenerator dungeon;

    private const float MinZoom = 0.25f;
    private const float MaxZoom = 4f;
    private float CurrentZoom = 1f;

    private float TileSize => BaseTileSize * CurrentZoom;

    public Dungeon2DView()
    {
        style.flexGrow = 1;
        focusable = true;

        generateVisualContent += OnGenerateVisualContent;

        RegisterCallback<MouseDownEvent>(OnMouseDown);
        RegisterCallback<WheelEvent>(OnWheel);
    }

    public void SetDungeon(Dungeon2DGenerator dungeon)
    {
        this.dungeon = dungeon;
        Refresh();
    }

    public void Refresh()
    {
        UpdateElementSize();
        MarkDirtyRepaint();
    }

    private void OnGenerateVisualContent(MeshGenerationContext context)
    {
        if (dungeon == null)
            return;

        Painter2D painter = context.painter2D;

        for (int y = 0; y < dungeon.Height; y++)
        {
            for (int x = 0; x < dungeon.Width; x++)
            {
                Rect rect = new(x * TileSize, (dungeon.Height - 1 - y) * TileSize, TileSize, TileSize);

                // Solid tiles
                painter.fillColor = GetTileColor(dungeon[x, y]);
                painter.BeginPath();
                painter.MoveTo(rect.min);
                painter.LineTo(new Vector2(rect.xMax, rect.yMin));
                painter.LineTo(rect.max);
                painter.LineTo(new Vector2(rect.xMin, rect.yMax));
                painter.ClosePath();
                painter.Fill();

                // Grid
                painter.strokeColor = Color.gray;
                painter.lineWidth = 1f;

                painter.BeginPath();
                painter.MoveTo(rect.min);
                painter.LineTo(new Vector2(rect.xMax, rect.yMin));
                painter.LineTo(rect.max);
                painter.LineTo(new Vector2(rect.xMin, rect.yMax));
                painter.ClosePath();
                painter.Stroke();
            }
        }
    }

    private Color GetTileColor(Dungeon2DTile tile)
    {
        return tile switch
        {
            Dungeon2DTile.Wall => new Color(0.15f, 0.15f, 0.15f),
            Dungeon2DTile.Hallway => new Color(0.8f, 0.8f, 0.8f),
            Dungeon2DTile.Room => new Color(0.8f, 0f, 0f),
            _ => Color.magenta
        };
    }

    private void OnWheel(WheelEvent wheelEvent)
    {
        if (!wheelEvent.ctrlKey)
            return;

        if (dungeon == null)
            return;

        float oldZoom = CurrentZoom;

        // Positive delta = zoom out, Negative delta = zoom in
        float zoomFactor = Mathf.Pow(1.1f, -wheelEvent.delta.y);
        CurrentZoom = Mathf.Clamp(CurrentZoom * zoomFactor, MinZoom, MaxZoom);

        if (Mathf.Approximately(oldZoom, CurrentZoom))
            return;

        Vector2 mousePosition = wheelEvent.localMousePosition;

        Vector2 dungeonPositionBeforeZoom = mousePosition / (BaseTileSize * oldZoom);

        UpdateElementSize();
        MarkDirtyRepaint();

        // Position the same dungeon point under the mouse after zooming.
        Vector2 newMousePosition = dungeonPositionBeforeZoom * TileSize;
        Vector2 difference = newMousePosition - mousePosition;

        ScrollView scrollView = GetFirstAncestorOfType<ScrollView>();
        if (scrollView != null)
        {
            Vector2 scrollOffset = scrollView.scrollOffset;
            scrollView.scrollOffset = scrollOffset + difference;
        }

        wheelEvent.StopPropagation();
    }

    private void OnMouseDown(MouseDownEvent mouseEvent)
    {
        if (dungeon == null)
            return;

        if (mouseEvent.button != 0)
            return;

        int x = Mathf.FloorToInt(mouseEvent.localMousePosition.x / TileSize);
        int flippedY = Mathf.FloorToInt(mouseEvent.localMousePosition.y / TileSize);
        int y = dungeon.Height - 1 - flippedY;

        if (x < 0 || x >= dungeon.Width || y < 0 || y >= dungeon.Height)
            return;

        Undo.RecordObject(dungeon, "Paint Dungeon Tile");

        dungeon[x, y] = dungeon[x, y] == Dungeon2DTile.Wall ? Dungeon2DTile.Hallway : Dungeon2DTile.Wall;

        EditorUtility.SetDirty(dungeon);
        MarkDirtyRepaint();
    }

    private void UpdateElementSize()
    {
        if(dungeon == null)
            return;

        style.width = dungeon.Width * TileSize;
        style.height = dungeon.Height * TileSize;
    }
}