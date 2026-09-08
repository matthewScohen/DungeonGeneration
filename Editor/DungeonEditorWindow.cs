using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DungeonEditorWindow : EditorWindow
{
    private DungeonObject DungeonObject;
    private DungeonGenerator DungeonGenerator;
    private DungeonView dungeonView;

    public static void ShowWindow(DungeonObject dungeon, DungeonGenerator dungeonGenerator = null)
    {
        DungeonEditorWindow window = GetWindow<DungeonEditorWindow>();

        window.titleContent = new GUIContent("Dungeon 2D Viewer");
        window.SetDungeon(dungeon);
        window.DungeonGenerator = dungeonGenerator;

        window.Refresh();

        if (dungeonGenerator != null)
            dungeonGenerator.Generated += window.Refresh;
    }

    private void SetDungeon(DungeonObject dungeon)
    {
        DungeonObject = dungeon;
        Refresh();
    }

    private void CreateGUI()
    {
        ScrollView scrollView = new();

        dungeonView = new();
        dungeonView.SetDungeon(DungeonObject);

        scrollView.Add(dungeonView);
        rootVisualElement.Add(scrollView);
    }

    private void OnEnable()
    {
        Undo.undoRedoPerformed += OnUndoRedo;
    }

    private void OnDisable()
    {
        Undo.undoRedoPerformed -= OnUndoRedo;
    }

    private void OnUndoRedo()
    {
        dungeonView?.Refresh();
    }

    private void Refresh()
    {
        if(DungeonGenerator != null && DungeonGenerator.DungeonObject != DungeonObject)
        {
            DungeonObject = DungeonGenerator.DungeonObject;
        }
        dungeonView?.SetDungeon(DungeonObject);
    }
}