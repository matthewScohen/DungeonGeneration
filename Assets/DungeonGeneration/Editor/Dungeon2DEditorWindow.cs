using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Dungeon2DEditorWindow : EditorWindow
{
    private Dungeon2DGenerator dungeon;
    private Dungeon2DView dungeon2DView;

    public static void ShowWindow(Dungeon2DGenerator dungeon)
    {
        Dungeon2DEditorWindow window = GetWindow<Dungeon2DEditorWindow>();

        window.titleContent = new GUIContent("Dungeon 2D Editor");
        window.SetDungeon(dungeon);

        window.Refresh();
    }

    private void SetDungeon(Dungeon2DGenerator newDungeon)
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

        dungeon2DView = new();
        dungeon2DView.SetDungeon(dungeon);

        scrollView.Add(dungeon2DView);
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
        dungeon2DView?.Refresh();
    }

    private void Refresh()
    {
        dungeon2DView?.SetDungeon(dungeon);
    }
}