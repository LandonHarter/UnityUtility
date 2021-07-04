using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnityUtilitySetup : EditorWindow
{

    bool installedPackages = false;

    public static void ShowSetup()
    {
        GetWindow<UnityUtilitySetup>("UnityUtility Setup");
    }

    void OnGUI()
    {
        if (GUILayout.Button("Import Required Packages"))
        {
            UnityUtilityEditor.InstallRequiredPackages();
            installedPackages = true;
        }

        if (GUILayout.Button("Finish"))
        {
            if (!installedPackages)
            {
                EditorUtility.DisplayDialog("Import Packages", "Please import required packages before finishing.", "Ok");
            }
            else
            {
                Close();
            }
        }
    }

}
