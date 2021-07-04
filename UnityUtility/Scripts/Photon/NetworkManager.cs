using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    [HideInInspector] public bool SetupProject = true;
    public static bool Connected = false;
    public static string Nickname;

    private static List<NetworkListener> listeners = new List<NetworkListener>();

    private static NetworkManager Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(this);
            Debug.LogWarning("Make sure there is only one Network Manager in the scene.");
        }

        Nickname = "Player" + Random.Range(1, 100000);
        PhotonNetwork.NickName = Nickname;
    }

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    void Update()
    {
        if (PhotonNetwork.NickName != Nickname)
        {
            for (int i = 0; i < listeners.Count; i++)
            {
                listeners[i].OnNicknameChanged(Nickname, PhotonNetwork.NickName);
            }
            Nickname = PhotonNetwork.NickName;
        }
    }

    public static void AddNetworkListener(NetworkListener listener)
    {
        listeners.Add(listener);
    }

    public static void RemoveNetworkListener(NetworkListener listener)
    {
        listeners.Remove(listener);
    }

    public static void ChangeNickname(string newNickname)
    {
        for (int i = 0; i < listeners.Count; i++)
        {
            listeners[i].OnNicknameChanged(Nickname, newNickname);
        }

        Nickname = newNickname;
        PhotonNetwork.NickName = Nickname;
    }

    public static void CreateRoom(string roomName, int maxPlayers)
    {
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = (byte) maxPlayers;
        ro.IsOpen = true;
        ro.IsVisible = true;

        Room room = new Room(roomName, ro);
        PhotonNetwork.JoinRoom(roomName);
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnConnectedToMaster()
    {
        for (int i = 0; i < listeners.Count; i++)
        {
            listeners[i].OnConnected();
        }

        Connected = true;
        PhotonNetwork.AutomaticallySyncScene = true; 
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        for (int i = 0; i < listeners.Count; i++)
        {
            listeners[i].OnDisconnected(cause);
        }

        Connected = false;
    }

    public override void OnJoinedRoom()
    {
        for (int i = 0; i < listeners.Count; i++)
        {
            listeners[i].OnJoinedRoom();
        }
    }

    public override void OnLeftRoom()
    {
        for (int i = 0; i < listeners.Count; i++)
        {
            listeners[i].OnLeftRoom();
        }
    }
}

public interface NetworkListener {

    void OnConnected();
    void OnDisconnected(DisconnectCause disconnectCause);
    void OnNicknameChanged(string oldNickname, string newNickname);
    void OnJoinedRoom();
    void OnLeftRoom();
    void OnChatMessageReceived(string sender, string message);

}
