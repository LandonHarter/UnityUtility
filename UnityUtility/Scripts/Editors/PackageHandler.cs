using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PackageHandler : AssetPostprocessor
{

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        if (importedAssets.Length > 0)
        {
            if (importedAssets[0] == "Assets/UnityUtility")
            {
                UnityUtilitySetup.ShowSetup();
            }
        }
    }

}
