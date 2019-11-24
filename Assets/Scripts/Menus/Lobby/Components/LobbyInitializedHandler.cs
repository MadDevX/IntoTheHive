using System;
using Zenject;

/// <summary>
/// This class handles SceneInitializedAnnouncer bound in LobbyInstaller.
/// Executes operations that require the lobby scene to be loaded and it's scene contexrt fully initialized.
/// </summary>
public class LobbyInitializedHandler : IInitializable, IDisposable
{
    private ClientInfo _clientInfo;
    private LobbyMessageSender _sender;
    private LobbyStateManager _lobbyStateManager;
    private SceneInitializedAnnouncer _lobbyAnnouncer;
    private SceneMessageWithResponse _sceneMessageWithResponse;

    public LobbyInitializedHandler(
        ClientInfo clientInfo,
        LobbyMessageSender sender,
        LobbyStateManager lobbyStateManager,
        SceneInitializedAnnouncer lobbyAnnouncer,
        SceneMessageWithResponse messageWithResponse
        )
    {
        _sender = sender;
        _clientInfo = clientInfo;
        _lobbyAnnouncer = lobbyAnnouncer;
        _lobbyStateManager = lobbyStateManager;
        _sceneMessageWithResponse = messageWithResponse;
    }

    public void Initialize()
    {
        _lobbyAnnouncer.SceneInitialized += HandleLobbyInitialized;
    }
    
    public void Dispose()
    {
        _lobbyAnnouncer.SceneInitialized -= HandleLobbyInitialized;
    }

    /// <summary>
    /// Executes post initilize operations
    /// </summary>
    private void HandleLobbyInitialized()
    {
        //A better solution would be to make a list of actions to be called here
        _sceneMessageWithResponse.SceneReady();

        if(_clientInfo.Status == ClientStatus.Host)
        {
            HostPostInitializeSetup();
        }

        ClientPostInitializeSetup();
    }

    private void ClientPostInitializeSetup()
    {
        //After the client's lobby scene is initialized for sure - it sends a request to the host to update his lobby state
        _sender.SendRequestLobbyUpdate();
    }

    private void HostPostInitializeSetup()
    {
        // In orded to make LobbyState lobby-only, the host application adds itself to the LobbyState only after the scene is properly initialized
        _lobbyStateManager.AddHostToLobby();
    }
}

