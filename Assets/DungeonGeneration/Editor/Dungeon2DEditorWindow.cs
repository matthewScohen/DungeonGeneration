using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Dungeon2DEditorWindow : EditorWindow
{
    private Dungeon2D dungeon;
    private Dungeon2DView dungeon2DView;

    public static void ShowWindow(Dungeon2D dungeon)
    {
        Dungeon2DEditorWindow window = GetWindow<Dungeon2DEditorWindow>();

        window.titleContent = new GUIContent("Dungeon 2D Editor");
        window.dungeon = dungeon;

        window.Refresh();
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
    }

    private void OnDisable()
    {
        Undo.undoRedoPerformed -= OnUndoRedo;
        AssetDatabase.SaveAssetIfDirty(dungeon);
    }

    private void OnUndoRedo()
    {
        dungeon2DView?.Refresh();
    }

    private void Refresh()
    {
        dungeon2DView.SetDungeon(dungeon);
    }
}