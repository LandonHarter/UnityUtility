using Steamworks;

public interface SteamCallbacks
{
    void OnInitialized();
    void OnInitializeFailed();
    void OnDisconnectedFromSteam();
    void OnOverlayShown();
    void OnOverlayHidden();
    void OnInviteSent(CSteamID user);
    void OnInviteReceived(CSteamID sender);
}
