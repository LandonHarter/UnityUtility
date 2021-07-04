#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;

[CustomEditor(typeof(ChatManager))]
public class ChatManagerEditor : Editor
{

    ChatManager cm;

    string currentChannelText = "";

    void Awake()
    {
        cm = (ChatManager)target;
    }

    public override void OnInspectorGUI()
    {

        GUILayout.Toggle(ChatManager.Connected, "Connected");

        GUILayout.Space(10);

        for (int i = 0; i < cm.channelsToSubTo.Count; i++)
        {
            string channelName = cm.channelsToSubTo[i];

            GUILayout.BeginHorizontal();

            GUILayout.Label(channelName);
            if (GUILayout.Button("Remove channel"))
            {
                cm.channelsToSubTo.Remove(channelName);
                if (Application.isPlaying) ChatManager.Unsubscribe(channelName);
                Debug.Log("Removed channel.");
            }

            GUILayout.EndHorizontal();
        }

        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        currentChannelText = EditorGUILayout.TextField("Add Channel", currentChannelText);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Add Channel") && !string.IsNullOrEmpty(currentChannelText) && !cm.channelsToSubTo.Contains(currentChannelText))
        {
            cm.channelsToSubTo.Add(currentChannelText);

            Debug.Log("Added channel.");
        }
        if (GUILayout.Button("Clear Channels"))
        {
            cm.channelsToSubTo.Clear();
            if (Application.isPlaying)
            {
                foreach (string channel in cm.channelsToSubTo)
                {
                    ChatManager.Unsubscribe(channel);
                }
            }
            Debug.Log("Cleared all channels.");
        }
    }

}
#endif