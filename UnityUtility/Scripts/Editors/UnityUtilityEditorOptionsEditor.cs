using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UnityUtilityEditorOptions))]
public class UnityUtilityEditorOptionsEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Set as Unity Utility Settings"))
        {
            UnityUtilityEditor.options = (UnityUtilityEditorOptions)target;
        }
    }

}