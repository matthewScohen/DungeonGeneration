using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Dungeon2DGenerator))]
public class Dungeon2DEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);

        Dungeon2DGenerator dungeon = (Dungeon2DGenerator)target;
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
            Dungeon2DEditorWindow.ShowWindow(dungeon);
        }
    }
}