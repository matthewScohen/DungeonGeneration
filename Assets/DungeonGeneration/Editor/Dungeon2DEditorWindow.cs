using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Dungeon2DEditorWindow : EditorWindow
{
    [MenuItem("Window/UI Toolkit/Dungeon2DEditorWindow")]
    public static void ShowExample()
    {
        Dungeon2DEditorWindow wnd = GetWindow<Dungeon2DEditorWindow>();
        wnd.titleContent = new GUIContent("Dungeon2DEditorWindow");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);

    }
}
