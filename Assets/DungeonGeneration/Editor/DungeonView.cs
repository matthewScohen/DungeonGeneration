using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DungeonView : VisualElement
{
    private const float BaseTileSize = 32f;

    private DungeonObject dungeonObject;
    private Dungeon Dungeon => dungeonObject == null ? null : dungeonObject.Dungeon;

    private const float MinZoom = 0.25f;
    private const float MaxZoom = 4f;
    private float CurrentZoom = 0.25f;

    private float TileSize => BaseTileSize * CurrentZoom;

    public DungeonView()
    {
        style.flexGrow = 1;
        focusable = true;

        generateVisualContent += OnGenerateVisualContent;
        
        RegisterCallback<WheelEvent>(OnWheel);
    }

    public void SetDungeon(DungeonObject dungeonObject)
    {
        this.dungeonObject = dungeonObject;
        Refresh();
    }

    public void Refresh()
    {
        UpdateElementSize();
        MarkDirtyRepaint();
    }

    private void OnGenerateVisualContent(MeshGenerationContext context)
    {
        if (Dungeon == null)
            return;

        Painter2D painter = context.painter2D;

        for (int y = 0; y < Dungeon.Height; y++)
        {
            for (int x = 0; x < Dungeon.Width; x++)
            {
                Rect rect = new(x * TileSize, (Dungeon.Height - 1 - y) * TileSize, TileSize, TileSize);

                // Solid tiles
                painter.fillColor = GetTileColor(Dungeon[x, y]);
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

    private Color GetTileColor(DungeonTile tile)
    {
        return tile switch
        {
            DungeonTile.Empty => new Color(0.15f, 0.15f, 0.15f),
            DungeonTile.Hallway => new Color(0.8f, 0.8f, 0.8f),
            DungeonTile.Room => new Color(0.8f, 0f, 0f),
            _ => Color.magenta
        };
    }

    private void OnWheel(WheelEvent wheelEvent)
    {
        if (!wheelEvent.ctrlKey)
            return;

        if (Dungeon == null)
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

    private void UpdateElementSize()
    {
        if(Dungeon == null)
            return;

        style.width = Dungeon.Width * TileSize;
        style.height = Dungeon.Height * TileSize;
    }
}