using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DungeonGenerator))]
public class DungeonGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);

        DungeonGenerator dungeonGenerator = (DungeonGenerator)target;
        using (new EditorGUI.DisabledScope(!dungeonGenerator.CanGenerate))
        {
            if (GUILayout.Button("Generate Dungeon", GUILayout.Height(30)))
            {
                Undo.RecordObject(dungeonGenerator, "Generate Dungeon");

                if (dungeonGenerator.Generate())
                    EditorUtility.SetDirty(dungeonGenerator);
            }
        }

        GUILayout.Space(5);

        if (GUILayout.Button("Open Dungeon Viewer", GUILayout.Height(30)))
        {
            DungeonEditorWindow.ShowWindow(dungeonGenerator.DungeonObject, dungeonGenerator);
        }

        GUILayout.Space(5);

        using (new EditorGUI.DisabledScope(dungeonGenerator.SavedDungeonObject == null))
        {
            if (GUILayout.Button("Save Dungeon Object", GUILayout.Height(30)))
            {
                string path = EditorUtility.SaveFilePanelInProject(
                    "Save Dungeon Object",
                    dungeonGenerator.name + " Object",
                    "asset",
                    "Choose where to save the generated dungeon object.");

                if (string.IsNullOrEmpty(path))
                    return;

                if (AssetDatabase.Contains(dungeonGenerator.SavedDungeonObject))
                {
                    EditorUtility.SetDirty(dungeonGenerator.SavedDungeonObject);
                }
                else
                {
                    AssetDatabase.CreateAsset(dungeonGenerator.SavedDungeonObject, path);
                }

                AssetDatabase.SaveAssets();
                EditorUtility.SetDirty(dungeonGenerator);
            }
        }
    }
}