using System;
using Zenject;

/// <summary>
/// Modifies LobbyState and has a public event which notifies when the readiness of all players change.
/// </summary>
public class LobbyStateManager: IInitializable
{
    private LobbyState _lobbyState;
    private GlobalHostPlayerManager _playerManager;
    private PlayerEntryManager _entryManager;
    private ProjectEventManager _eventManager;
    public event Action<bool> AllPlayersReadyChanged;

    public LobbyStateManager(
        LobbyState lobbyState,
        GlobalHostPlayerManager playerManager,
        PlayerEntryManager entryManager)
    {
        _lobbyState = lobbyState;
        _playerManager = playerManager;
        _entryManager = entryManager;
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
    public void AddPlayerToLobby(ushort id, string nickname, bool isReady = false)
    {
        _entryManager.SetReady(nickname, id, isReady);
        if (_lobbyState.PlayersReadyStatus.ContainsKey(id))
        {
            _lobbyState.PlayersReadyStatus[id].Ready = isReady;
        }
        else
        {
            _lobbyState.PlayersReadyStatus.Add(id, new LobbyPlayerData(nickname, isReady));
        }

        AreAllPlayersReady();
    }

    /// <summary>
    /// Adds a player to the lobby's ready players list.
    /// This method also invokes AllPlayersReadyChanged.
    /// </summary>
    /// <param name="player">Id and nickname of the client to add to the lobby</param>
    /// <param name="isReady">Client readiness</param>
    public void AddPlayerToLobby(ConnectedPlayerData player, bool isReady = false)
    {
        AddPlayerToLobby(player.ID, player.Nickname, isReady);
    }

    /// <summary>
    /// Checks if all players are ready and invokes AllPlayersReadyChanged with the current value.
    /// </summary>
    public void AreAllPlayersReady()
    {
        bool allReady = true;
        foreach (LobbyPlayerData value in _lobbyState.PlayersReadyStatus.Values)
        {
            allReady = allReady && value.Ready;
        }

        AllPlayersReadyChanged?.Invoke(allReady);
    }

    public void SetReady(ushort id, bool isReady)
    {
        if(_lobbyState.PlayersReadyStatus.TryGetValue(id, out var lobbyPlayerData))
        {
            AddPlayerToLobby(id, lobbyPlayerData.Nickname, isReady);
        }
        else
        {
            throw new ArgumentException("Did not find a player with this id in the lobby.");
        }
    }
}

