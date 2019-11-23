using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

//TODO MG: Should this menu be split into ConnectionMenuManager and NetworkedMenuManager??
public class ConnectionMenuManager : IInitializable, IDisposable
{
    private Button _serverButton;
    private Button _joinButton;
    private Button _backButton;

    private ClientInfo _clientInfo;
    private ServerManager _serverManager; 
    private NetworkedClientInitializer _initializer; 
    private ConnectionMenuMessageSender _connectionMenuMessageSender;

    public ConnectionMenuManager(
        [Inject(Id = Identifiers.ConnetionMenuCreateServerButton)] Button serverButton,
        [Inject(Id = Identifiers.ConnetionMenuJoinServerButton)] Button joinButton,
        [Inject(Id = Identifiers.ConnetionMenuBackButton)] Button backButton,
        ConnectionMenuMessageSender connectionMenuMessageSender,
        NetworkedClientInitializer connectionInitializer,
        ServerManager serverManager,
        ClientInfo clientInfo)
    {
        _joinButton = joinButton;
        _backButton = backButton;
        _serverButton = serverButton;

        _clientInfo = clientInfo;
        _serverManager = serverManager;
        _initializer = connectionInitializer;
        _connectionMenuMessageSender = connectionMenuMessageSender;
    }

    public void Initialize()
    {
        // When the client application initializes this scene, 
        // its client/host state is reset to not be treated as either one
        _clientInfo.ResetState();
        _serverButton.onClick.AddListener(ServerButtonClicked);
        _joinButton.onClick.AddListener(JoinButtonClicked);
        _backButton.onClick.AddListener(BackButtonClicker);
    }

    public void Dispose()
    {
        _serverButton.onClick.RemoveListener(ServerButtonClicked);
        _joinButton.onClick.RemoveListener(JoinButtonClicked);
        _backButton.onClick.RemoveListener(BackButtonClicker);
    }

    public void ServerButtonClicked()
    {
        // 1. Server is created
        _serverManager.CreateServer();
        // 2. JoinedServerAsX is subscribed to clientInfo.StatusChanged
        _clientInfo.StatusChanged += JoinedServerAsHost;
        // 3. Client joins the server
        _serverManager.JoinAsHost();
        // 4. Server sends us connectionInfo message
        // 5. ClientInfo updates its status
        // 6. JoinedServerAsX fires
        // 7. Scene is loaded and the event is unsubscribed
    }

    public void JoinButtonClicked()
    {
        // 1. Subscribe to ClientInfo status update
        _clientInfo.StatusChanged += JoinedServerAsClient;
        // 2. Join the server
        _initializer.JoinServer();
        // 3. Get the client status
        // 4. Send request for scene update
        // 5. Handle the response in Networked Scene Manager
    }

    public void BackButtonClicker()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void JoinedServerAsClient(ushort status)
    {
        if (status == ClientStatus.Client)
        {
            // This message is handled in HostLobbyManager
            // In response - LoadLobby Message is sent
            _connectionMenuMessageSender.PlayerJoinedMessage();
        }
        else
        {
            Debug.Log("Unable to join the server. Try again");
        }
        _clientInfo.StatusChanged -= JoinedServerAsClient;
    }

    private void JoinedServerAsHost(ushort status)
    {
        if (status == ClientStatus.Host)
        {
            // This message is handled in HostLobbyManager
            // In response - LoadLobby Message is sent

            // Now the functionality on Parsing PlayerJoined message is moved from Connection menu to LobbyInitializedHandler if the clientStatus is host
            //_connectionMenuMessageSender.PlayerJoinedMessage();
            _connectionMenuMessageSender.PlayerJoinedMessage();

            //SceneManager.LoadScene("Lobby");
        }
        else
        {
            Debug.Log("Server setup unsuccessful. Try again");
            _serverManager.CloseServer();
        }
        _clientInfo.StatusChanged -= JoinedServerAsHost;
    }

}