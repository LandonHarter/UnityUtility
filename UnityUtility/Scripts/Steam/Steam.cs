using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Steam : MonoBehaviour, SteamCallbacks
{

    [Header("Options")]

    public bool useSteam = true;
    public bool logEventsInConsole = true;

    public static bool logEvents;

    private bool overlayShowing = false;

    public static List<SteamCallbacks> callbacks = new List<SteamCallbacks>();

    //Callbacks
    protected Callback<GameRichPresenceJoinRequested_t> inviteReceived;
    protected Callback<GameOverlayActivated_t> overlayActivatedCallback;
    protected Callback<SteamServersDisconnected_t> disconnected;

    void Awake()
    {
        logEvents = logEventsInConsole;
        AddCallback(this);

        disconnected = Callback<SteamServersDisconnected_t>.Create(OnDisconnected);
        inviteReceived = Callback<GameRichPresenceJoinRequested_t>.Create(OnInviteReceived);
        overlayActivatedCallback = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
    }

    #region Callback Methods

    private void OnDisconnected(SteamServersDisconnected_t param)
    {
        foreach (SteamCallbacks callback in callbacks)
        {
            callback.OnDisconnectedFromSteam();
        }
    }

    private void OnGameOverlayActivated(GameOverlayActivated_t param)
    {
        overlayShowing = !overlayShowing;
        foreach (SteamCallbacks callback in callbacks)
        {
            if (overlayShowing) callback.OnOverlayShown();
            if (!overlayShowing) callback.OnOverlayHidden();
        }
    }

    private void OnInviteReceived(GameRichPresenceJoinRequested_t param)
    {
        foreach (SteamCallbacks callback in callbacks)
        {
            callback.OnInviteReceived(param.m_steamIDFriend);
        }
    }

    #endregion

    void Start()
    {
        if (!useSteam) return;

        if (!SteamManager.Initialized)
        {
            foreach (SteamCallbacks callback in callbacks)
            {
                callback.OnInitializeFailed();
            }

            return;
        }

        foreach (SteamCallbacks callback in callbacks)
        {
            callback.OnInitialized();
        }
    }

    public static void InviteFriend(CSteamID friend)
    {
        bool success = SteamFriends.InviteUserToGame(friend, "Invite from " + SteamFriends.GetPersonaName());
        if (success)
        {
            foreach (SteamCallbacks callback in callbacks)
            {
                callback.OnInviteSent(friend);
            }
        }
    }

    public static void AddCallback(SteamCallbacks callback)
    {
        callbacks.Add(callback);
    }

    public void OnInitialized()
    {
        if (logEventsInConsole) Debug.Log("Sucessfully connected to steam.");
    }

    public void OnInitializeFailed()
    {
        if (logEventsInConsole) Debug.LogError("Couldn't connect to steam. Steam must be open for this to work. If it is open, try fixing your app id in the Steam/Steam Settings window and try again.");
    }

    public void OnDisconnectedFromSteam()
    {
        if (logEventsInConsole) Debug.Log("Disconnected from steam.");
    }

    public void OnOverlayShown()
    {
        if (logEventsInConsole) Debug.Log("Overlay shown.");
    }

    public void OnOverlayHidden()
    {
        if (logEventsInConsole) Debug.Log("Overlay hidden.");
    }

    public void OnInviteSent(CSteamID user)
    {
        if (logEventsInConsole) Debug.Log("Sent invite to " + SteamFriends.GetFriendPersonaName(user));
    }

    public void OnInviteReceived(CSteamID sender)
    {
        if (logEventsInConsole) Debug.Log("Received invite from " + SteamFriends.GetFriendPersonaName(sender) + ".");
    }
}
