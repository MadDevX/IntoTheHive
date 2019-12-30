using System;
using Zenject;

/// <summary>
/// Handles host specific logic that happens after the scene is initialized
/// Reply to change scene message takes place in post initialization and this class reacts to event later event.
/// </summary>
public class LobbyInitializer : IInitializable, IDisposable
{
    private ProjectEventManager _eventManager;
    private LobbyMessageSender _lobbyMessageSender;

    public LobbyInitializer(
        ProjectEventManager eventManager,
        LobbyMessageSender lobbyMessageSender)
    {
        _eventManager = eventManager;
        _lobbyMessageSender = lobbyMessageSender;
    }

    public void Initialize()
    {
        _eventManager.LobbyInitializedHost += RefreshLobby;
    }

    public void Dispose()
    {
        _eventManager.LobbyInitializedHost -= RefreshLobby;
    }

    private void RefreshLobby()
    {
        _lobbyMessageSender.SendUpdateLobbyMessage();
    }
}