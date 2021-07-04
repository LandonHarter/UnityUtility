using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UnityUtilityEditor : EditorWindow
{

    public static UnityUtilityEditorOptions options;
    private static Object options_O;

    void Awake()
    {
        if (options == null) options = FindObjectOfType<UnityUtilityEditorOptions>();
    }

    void Update()
    {
        if (EditorGUIUtility.GetObjectPickerObject() != null)
        {
            Object obj = EditorGUIUtility.GetObjectPickerObject();

            if (obj.GetType().Equals(typeof(UnityUtilityEditorOptions)) && options != obj)
            {
                options = (UnityUtilityEditorOptions)obj;
                options_O = null;
            }
        }
    }

    [MenuItem("Unity Utility/Setup", priority = 0)]
    public static void OpenSettings()
    {
        GetWindow<UnityUtilityEditor>();
    }

    void OnGUI()
    {
        if (GUILayout.Button("Choose Settings Prefab"))
        {
            EditorGUIUtility.ShowObjectPicker<UnityUtilityEditorOptions>(options_O, false, "", 0);
        }
    }

    [MenuItem("Unity Utility/Create Terrain")]
    public static void CreateTerrain()
    {
        GameObject terrain = new GameObject();
        terrain.name = "New Terrain";
        terrain.AddComponent<MeshFilter>();
        MeshRenderer mr = terrain.AddComponent<MeshRenderer>();
        TerrainGenerator g = terrain.AddComponent<TerrainGenerator>();
        mr.sharedMaterial = options.terrainMaterial;
        g.enabled = true;
        g.GenerateMesh();
    }

}

[CreateAssetMenu(menuName = "Unity Utility/Settings")]
public class UnityUtilityEditorOptions : ScriptableObject {

    public Material terrainMaterial;
    
}