using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PackageHandler : AssetPostprocessor
{

    private static bool openedSetupAlready = false;

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        bool isUnityUtility = false;
        if (importedAssets.Length > 0)
        {
            if (importedAssets[0].Contains("UnityUtility"))
            {
                isUnityUtility = true;
            }
        }

        if (isUnityUtility && !openedSetupAlready)
        {
            UnityUtilitySetup.ShowSetup();
            openedSetupAlready = true;
        }
    }

}
