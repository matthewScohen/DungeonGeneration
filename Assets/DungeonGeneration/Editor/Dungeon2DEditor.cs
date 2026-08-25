using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Dungeon2D))]
public class Dungeon2DEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);

        if (GUILayout.Button("Open Dungeon Editor", GUILayout.Height(30)))
        {
            Dungeon2DEditorWindow.ShowWindow((Dungeon2D)target);
        }
    }
}