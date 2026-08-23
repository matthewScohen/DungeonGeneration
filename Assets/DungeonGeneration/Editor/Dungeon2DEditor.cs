using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(Dungeon2D))]
public class Dungeon2DEditor : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        VisualElement myInspector = new();

        myInspector.Add(new Label("This is a custom Inspector"));

        return myInspector;
    }
}
