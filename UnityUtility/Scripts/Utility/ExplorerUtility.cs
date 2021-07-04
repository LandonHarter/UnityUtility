using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExplorerUtility
{
  
    public static void ShowInExplorer(string path)
    {
        Application.OpenURL(path);
    }

}
