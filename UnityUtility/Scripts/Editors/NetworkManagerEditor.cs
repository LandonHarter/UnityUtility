#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Photon.Pun;

[CustomEditor(typeof(NetworkManager))]
public class NetworkManagerEditor : Editor
{

    NetworkManager nm;

    void Awake()
    {
        nm = (NetworkManager)target;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Toggle(NetworkManager.Connected, "Connected");
    }

}
#endif