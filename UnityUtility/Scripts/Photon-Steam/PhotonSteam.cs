using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonSteam : MonoBehaviour
{

    private static PhotonSteam Instance;
    private List<CSteamID> playersInParty = new List<CSteamID>();

    protected Callback<GameRichPresenceJoinRequested_t> gameRichPresenceRequsted;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Debug.Log("Can only have one instance of the PhotonSteam script.");
            Destroy(this);
        }
    }

    void OnEnable()
    {
        gameRichPresenceRequsted = Callback<GameRichPresenceJoinRequested_t>.Create(OnGameRichPresenceJoinRequested);
    }

    public static void InvitePlayer(CSteamID id)
    {
        bool success = SteamFriends.InviteUserToGame(id, "");
        if (success) Debug.Log("Invited " + SteamFriends.GetFriendPersonaName(id));
    }

    private void OnGameRichPresenceJoinRequested(GameRichPresenceJoinRequested_t callback)
    {
        Debug.Log("Invite accepted");
        playersInParty.Add(callback.m_steamIDFriend);
    }
}
