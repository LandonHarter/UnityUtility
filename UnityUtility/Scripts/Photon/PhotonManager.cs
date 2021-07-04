using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonManager : MonoBehaviour
{

    void Awake()
    {
        GameObject networkManager = new GameObject();
        networkManager.name = "Network Manager";
        networkManager.AddComponent<NetworkManager>();
        networkManager.AddComponent<ChatManager>();
        networkManager.AddComponent<PhotonUtility>();
        DontDestroyOnLoad(networkManager);
    }

}
