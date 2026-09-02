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

        if (GUILayout.Button("Open Dungeon Viewer", GUILayout.Height(30)))
        {
            DungeonEditorWindow.ShowWindow(dungeon);
        }

        GUILayout.Space(5);

        using (new EditorGUI.DisabledScope(dungeon.SavedDungeonObject == null))
        {
            if (GUILayout.Button("Save Dungeon Object", GUILayout.Height(30)))
            {
                string path = EditorUtility.SaveFilePanelInProject(
                    "Save Dungeon Object",
                    dungeon.name + " Object",
                    "asset",
                    "Choose where to save the generated dungeon object.");

                if (string.IsNullOrEmpty(path))
                    return;

                if (AssetDatabase.Contains(dungeon.SavedDungeonObject))
                {
                    EditorUtility.SetDirty(dungeon.SavedDungeonObject);
                }
                else
                {
                    AssetDatabase.CreateAsset(dungeon.SavedDungeonObject, path);
                }

                AssetDatabase.SaveAssets();
                EditorUtility.SetDirty(dungeon);
            }
        }
    }
}