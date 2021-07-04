#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : EditorWindow
{
    
    [MenuItem("Unity Utility/Scene Switcher")]
    private static void ShowWindow()
    {
        GetWindow<SceneSwitcher>("Scene Switcher");
    }

    void OnGUI()
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            if (GUILayout.Button(GetSceneNameFromScenePath(path)))
            {
                EditorSceneManager.OpenScene(path);
                Debug.Log("Scene Switcher: Loaded " + GetSceneNameFromScenePath(path));
            }
        }
    }

    private string GetSceneNameFromScenePath(string scenePath)
    {
        var sceneNameStart = scenePath.LastIndexOf("/", StringComparison.Ordinal) + 1;
        var sceneNameEnd = scenePath.LastIndexOf(".", StringComparison.Ordinal);
        var sceneNameLength = sceneNameEnd - sceneNameStart;
        return scenePath.Substring(sceneNameStart, sceneNameLength);
    }
}
#endif