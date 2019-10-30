using System.Collections.Generic;

/// <summary>
/// This class holds the current state of the lobby in the form of clientId (ushort) - isReady (boolean) 
/// This class functionality is used only on the host version of the client 
/// </summary>
public class LobbyState 
{
    public Dictionary<ushort, bool> PlayersReadyStatus;

    public LobbyState()
    {
        PlayersReadyStatus = new Dictionary<ushort, bool>();
    }
}