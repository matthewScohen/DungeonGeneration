using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DungeonObject))]
public class DungeonObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);

        DungeonObject dungeon = (DungeonObject)target;

        if (GUILayout.Button("Open Dungeon Viewer", GUILayout.Height(30)))
        {
            DungeonEditorWindow.ShowWindow(dungeon);
        }
    }

}