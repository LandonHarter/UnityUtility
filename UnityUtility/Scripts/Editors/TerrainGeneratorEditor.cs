#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainGenerator))]
public class TerrainGeneratorEditor : Editor
{

    private TerrainGenerator terrain;

    void Awake()
    {
        terrain = (TerrainGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate Mesh"))
        {
            terrain.GenerateMesh();
        }
        if (GUILayout.Button("Clear Mesh"))
        {
            terrain.ClearMesh();
        }
    }

}
#endif