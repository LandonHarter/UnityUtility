using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChatManager : MonoBehaviour, IChatClientListener
{

    public static ChatManager Instance;
    private ChatClient client;

    [HideInInspector] public List<string> channelsToSubTo = new List<string>();

    public static bool Connected = false;

    private static List<ChatListener> chatListeners = new List<ChatListener>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Debug.LogWarning("Make sure there is only one chat manager in the scene");
            Destroy(this);
        }

        Init();
    }

    public void Init()
    {
        client = new ChatClient(this);
        client.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(PhotonNetwork.NickName));
    }

    void Update()
    {
        client.Service();
    }

    public static void Subscribe(string channel)
    {
        if (Instance.channelsToSubTo.Contains(channel)) return;

        Instance.channelsToSubTo.Add(channel);
        Instance.client.Subscribe(channel);
    }

    public static void Unsubscribe(string channel)
    {
        if (!Instance.channelsToSubTo.Contains(channel)) return;

        Instance.channelsToSubTo.Remove(channel);
        Instance.client.Unsubscribe(new string[] { channel });
    }

    public static void AddListener(ChatListener listener)
    {
        chatListeners.Add(listener);
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        
    }

    public void OnChatStateChange(ChatState state)
    {
        
    }

    public void OnConnected()
    {
        Connected = true;

        client.Subscribe(channelsToSubTo.ToArray());

        foreach (ChatListener listener in chatListeners)
        {
            listener.OnConnected();
        }
    }

    public void OnDisconnected()
    {
        Connected = false;

        foreach (ChatListener listener in chatListeners)
        {
            listener.OnDisconnected();
        }
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        Debug.Log("Subscribed to new channel.");

        foreach (ChatListener listener in chatListeners)
        {
            listener.OnSubscribed();
        }
    }

    public void OnUnsubscribed(string[] channels)
    {
        Debug.Log("Unscribed from channel");

        foreach (ChatListener listener in chatListeners)
        {
            listener.OnUnsubscribed();
        }
    }

    public void OnUserSubscribed(string channel, string user)
    {
        
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        
    }
}

public interface ChatListener
{

    void OnConnected();
    void OnDisconnected();
    void OnSubscribed();
    void OnUnsubscribed();

}