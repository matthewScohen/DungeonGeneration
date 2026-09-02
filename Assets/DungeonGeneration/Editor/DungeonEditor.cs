using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DungeonGenerator))]
public class DungeonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);

        DungeonGenerator dungeon = (DungeonGenerator)target;
        using (new EditorGUI.DisabledScope(!dungeon.CanGenerate))
        {
            if (GUILayout.Button("Generate Dungeon", GUILayout.Height(30)))
            {
                Undo.RecordObject(dungeon, "Generate Dungeon");

                if (dungeon.Generate())
                    EditorUtility.SetDirty(dungeon);
            }
        }

        GUILayout.Space(5);

        if (GUILayout.Button("Open Dungeon Editor", GUILayout.Height(30)))
        {
            DungeonEditorWindow.ShowWindow(dungeon);
        }
    }
}