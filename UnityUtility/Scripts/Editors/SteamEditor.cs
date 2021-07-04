#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Steam))]
public class SteamEditor : Editor
{

    public static int setup = 0;

    void OnEnable()
    {
        setup = File.Exists(Regex.Split(Application.dataPath, "Assets")[0] + "steam_appid.txt") ? 1 : 0;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Setup Steam"))
        {
            if (setup == 0)
            {
                string path = Regex.Split(Application.dataPath, "Assets")[0] + "steam_appid.txt";
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    byte[] data = Encoding.Default.GetBytes("480");
                    fs.Write(data, 0, data.Length);
                }

                setup = 1;
            }
            else
            {
                Debug.Log("Steam has already been setup.");
            }
        }
    }

}
#endif