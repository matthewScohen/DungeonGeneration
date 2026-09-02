using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DungeonEditorWindow : EditorWindow
{
    private DungeonGenerator dungeon;
    private DungeonView dungeonView;

    public static void ShowWindow(DungeonGenerator dungeon)
    {
        DungeonEditorWindow window = GetWindow<DungeonEditorWindow>();

        window.titleContent = new GUIContent("Dungeon 2D Editor");
        window.SetDungeon(dungeon);

        window.Refresh();
    }

    private void SetDungeon(DungeonGenerator newDungeon)
    {
        if (dungeon != null)
            dungeon.Generated -= Refresh;

        dungeon = newDungeon;

        if (dungeon != null)
            dungeon.Generated += Refresh;
    }

    private void CreateGUI()
    {
        ScrollView scrollView = new();

        dungeonView = new();
        dungeonView.SetDungeon(dungeon);

        scrollView.Add(dungeonView);
        rootVisualElement.Add(scrollView);
    }

    private void OnEnable()
    {
        Undo.undoRedoPerformed += OnUndoRedo;

        if (dungeon != null)
            dungeon.Generated += Refresh;
    }

    private void OnDisable()
    {
        Undo.undoRedoPerformed -= OnUndoRedo;

        if (dungeon != null)
            dungeon.Generated -= Refresh;

        AssetDatabase.SaveAssetIfDirty(dungeon);
    }

    private void OnUndoRedo()
    {
        dungeonView?.Refresh();
    }

    private void Refresh()
    {
        dungeonView?.SetDungeon(dungeon);
    }
}