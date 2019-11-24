using System;
using Zenject;

/// <summary>
/// Modifies LobbyState and has a public event which notifies when the readiness of all players change.
/// </summary>
public class LobbyStateManager: IInitializable
{
    private LobbyState _lobbyState;
    private GlobalHostPlayerManager _playerManager;
    public event Action<bool> AllPlayersReadyChanged;

    public LobbyStateManager(
        LobbyState lobbyState,
        GlobalHostPlayerManager playerManager)
    {
        _lobbyState = lobbyState;
        _playerManager = playerManager;
    }

    /// <summary>
    /// Add currently connected players (most notably HOST) with not ready status.
    /// </summary>
    public void Initialize()
    {
        _playerManager.ConnectedPlayers.ForEach(player => AddPlayerToLobby(player));
    }

    /// <summary>
    /// Adds a player to the lobby's ready players list.
    /// This method also invokes AllPlayersReadyChanged.
    /// </summary>
    /// <param name="id"> Id of the client to add to the lobby</param>
    /// <param name="isReady">Client readiness</param>
    public void AddPlayerToLobby(ushort id, bool isReady = false)
    {
        if (_lobbyState.PlayersReadyStatus.ContainsKey(id))
        {
            _lobbyState.PlayersReadyStatus[id] = isReady;
        }
        else
        {
            _lobbyState.PlayersReadyStatus.Add(id, isReady);
        }

        AreAllPlayersReady();
    }

    /// <summary>
    /// Checks if all players are ready and invokes AllPlayersReadyChanged with the current value.
    /// </summary>
    public void AreAllPlayersReady()
    {
        bool allReady = true;
        foreach (bool value in _lobbyState.PlayersReadyStatus.Values)
        {
            allReady = allReady && value;
        }

        AllPlayersReadyChanged?.Invoke(allReady);
    }

   
}

