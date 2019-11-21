using System.Collections.Generic;

public class LobbyState 
{
    public Dictionary<ushort, bool> PlayersReadyStatus;
    public bool IsAllPlayersReady;

    public LobbyState()
    {
        PlayersReadyStatus = new Dictionary<ushort, bool>();
        IsAllPlayersReady = false;
    }
}