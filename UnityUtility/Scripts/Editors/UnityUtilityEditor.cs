using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager;

public class UnityUtilityEditor : EditorWindow
{

    private static bool Installing;
    private static AddRequest Request;
    private static ListRequest ListRequest;
    private static string currentInstallingPackage;

    private static List<string> pendingPackagesToInstall = new List<string>();

    private static List<string> requiredPackages = new List<string>() { 
        "com.unity.animation.rigging", 
        "com.unity.render-pipelines.universal",
        "com.unity.textmeshpro",
     };

    [MenuItem("Unity Utility/Install or Update Packages", priority = 0)]
    public static void InstallRequiredPackages()
    {
        foreach (string package in requiredPackages)
        {
            InstallPackage(package);
        }
    }

    private static void InstallPackage(string packageName)
    {
        if (Installing)
        {
            pendingPackagesToInstall.Add(packageName);
            return;
        }

        currentInstallingPackage = packageName;

        Installing = true;

        ListRequest = Client.List();
        EditorApplication.update += CheckPackageList;

        Request = Client.Add(packageName);
        EditorApplication.update += PackageInstallationProgress;
    }

    private static void UninstallPackage(string packageName)
    {
        Client.Remove(packageName);
    }

    private static void PackageInstallationProgress()
    {
        if (Request.IsCompleted)
        {
            if (Request.Status == StatusCode.Success)
                Debug.Log("Installed: " + Request.Result.packageId);
            else if (Request.Status >= StatusCode.Failure)
                Debug.Log(Request.Error.message);

            currentInstallingPackage = string.Empty;
            EditorApplication.update -= PackageInstallationProgress;
            Installing = false;

            if (pendingPackagesToInstall.Count > 0)
            {
                string packageName = pendingPackagesToInstall[0];
                pendingPackagesToInstall.RemoveAt(0);
                InstallPackage(packageName);
            }
        }
    }

    private static void CheckPackageList()
    {
        if (ListRequest.IsCompleted)
        {
            if (Request.Status == StatusCode.Success)
                foreach (var package in ListRequest.Result)
                {
                    if (package.name == currentInstallingPackage)
                    {
                        Debug.Log(currentInstallingPackage + " is already installed.");
                        return;
                    }
                }
            else if (Request.Status >= StatusCode.Failure)
            {
                Debug.Log(Request.Error.message);
                Installing = false;
            }
        }

        EditorApplication.update -= CheckPackageList;
    }

    [MenuItem("Unity Utility/Create Terrain")]
    private static void CreateTerrain()
    {
        GameObject terrain = new GameObject();
        terrain.name = "New Terrain";
        terrain.AddComponent<MeshFilter>();
        terrain.AddComponent<MeshRenderer>();
        TerrainGenerator g = terrain.AddComponent<TerrainGenerator>();
        g.enabled = true;
        g.GenerateMesh();
    }

}