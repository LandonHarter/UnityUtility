#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Text.RegularExpressions;
using System.Text;

public class SteamSettings : EditorWindow
{

    private static string appIdFilePath;
    private string currentID;

    void Awake()
    {
        appIdFilePath = Regex.Split(Application.dataPath, "Assets")[0] + "steam_appid.txt";
        using (FileStream appIdFileStream = new FileStream(appIdFilePath, FileMode.Open))
        {
            byte[] data = new byte[(int)appIdFileStream.Length];
            appIdFileStream.Read(data, 0, (int)appIdFileStream.Length);

            currentID = Encoding.UTF8.GetString(data);
        }
    }

    [MenuItem("Steam/Steam Settings")]
    private static void ShowWindow()
    {
        if (SteamEditor.setup == 1) GetWindow<SteamSettings>("Steam Settings");
        else if (SteamEditor.setup == 0) Debug.Log("Make sure the Steam script is setup and in your scene before editing settings");
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space((position.width / 2) - 45);
        GUILayout.Label("App ID Settings", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        currentID = EditorGUILayout.TextField("App ID", currentID);

        if (GUILayout.Button("Change App ID"))
        {
            File.WriteAllText(appIdFilePath, string.Empty);
            using (FileStream appIdFileStream = new FileStream(appIdFilePath, FileMode.Open))
            {
                byte[] idData = Encoding.Default.GetBytes(currentID);
                appIdFileStream.Write(idData, 0, idData.Length);
            }
        }
        if (GUILayout.Button("Choose Non-Steam Game"))
        {
            string path = "C://Program Files (x86)//Steam//userdata";

            string dir = Directory.GetDirectories(path)[0];
            dir += "//760//screenshots.vdf";
            path = dir;

            ExplorerUtility.ShowInExplorer(path);
        }
        if (GUILayout.Button("Reset"))
        {
            File.WriteAllText(appIdFilePath, string.Empty);
            using (FileStream appIdFileStream = new FileStream(appIdFilePath, FileMode.Open))
            {
                byte[] idData = Encoding.Default.GetBytes("480");
                appIdFileStream.Write(idData, 0, idData.Length);

                currentID = "480";
            }
        }
    }

}
#endif