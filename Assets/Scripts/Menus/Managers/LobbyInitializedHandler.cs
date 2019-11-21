using System;
using Zenject;

public class LobbyInitializedHandler : IInitializable, IDisposable
{
    private SceneInitializedAnnouncer _lobbyAnnouncer;
    private ClientInfo _clientInfo;
    private ClientLobbyManager _clientLobbyManager;
    private HostLobbyManager _hostLobbyManager;

    public LobbyInitializedHandler(
        SceneInitializedAnnouncer lobbyAnnouncer,
        ClientInfo clientInfo)
    {
        _clientInfo = clientInfo;
        _lobbyAnnouncer = lobbyAnnouncer;
    }

    public void Initialize()
    {
        _lobbyAnnouncer.SceneInitialized += HandleLobbyInitialized;
    }
    
    public void Dispose()
    {
        _lobbyAnnouncer.SceneInitialized -= HandleLobbyInitialized;
    }

    private void HandleLobbyInitialized()
    {
        if(_clientInfo.Status == ClientStatus.Host)
        {
            HostSetup();
        }

        ClientSetup();
    }

    private void ClientSetup()
    {
        _clientLobbyManager.RequestLobbyUpdate();
        
    }

    private void HostSetup()
    {
        //Host is always the first player connected to the server
        _hostLobbyManager.AddNewPlayer(0);
        //add hsot to hostglobalplayers
        //maybe just send a joined message here?
    }
}

