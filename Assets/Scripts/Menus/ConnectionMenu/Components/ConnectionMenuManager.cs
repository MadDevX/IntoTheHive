using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

/// <summary>
/// This class adds handlers to the menu buttons and implements the connection setup
/// </summary>
public class ConnectionMenuManager : IInitializable, IDisposable
{
    private Button _serverButton;
    private Button _joinButton;
    private Button _backButton;
    private Text _serverButtonText;

    private ClientInfo _clientInfo;
    private ServerManager _serverManager; 
    private ClientConnectionInitializer _initializer; 
    private ConnectionMenuMessageSender _connectionMenuMessageSender;

    public ConnectionMenuManager(
        [Inject(Id = Identifiers.ConnetionMenuCreateServerButton)] Button serverButton,
        [Inject(Id = Identifiers.ConnetionMenuJoinServerButton)] Button joinButton,
        [Inject(Id = Identifiers.ConnetionMenuBackButton)] Button backButton,
        ClientInfo clientInfo,
        ServerManager serverManager,
        ClientConnectionInitializer connectionInitializer,
        ConnectionMenuMessageSender connectionMenuMessageSender)
    {
        _joinButton = joinButton;
        _backButton = backButton;
        _serverButton = serverButton;

        _clientInfo = clientInfo;
        _serverManager = serverManager;
        _initializer = connectionInitializer;
        _connectionMenuMessageSender = connectionMenuMessageSender;

        _serverButtonText = _serverButton.GetComponentInChildren<Text>();
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
        // 2. JoinedServerAsHost is subscribed to clientInfo.StatusChanged
        // 3. Client joins the server
        // 4. Server sends us connectionInfo message
        // 5. ClientInfo updates its status
        // 6. JoinedServerAsX fires
        // 7. Scene is loaded and the event is unsubscribed
        if (_serverManager.CreateServer())
        {
            _clientInfo.SubscribeOnStatusChanged(JoinedServerAsHost);
            _serverManager.JoinAsHost();
        }
        else
        {
            _serverButtonText.text = "Port busy!";
            _serverButton.interactable = false;
            //TODO: use coroutine to bring back functionality after 3 seconds
        }
    }

    public void JoinButtonClicked()
    {
        // 1. Subscribe to ClientInfo status update
        // 2. Join the server
        // 3. Get the client status
        // 4. Update the ClientInfo status and fire JoinedServerAsClient
        // 4. Send request for scene update
        // 5. Handle the response in Networked Scene Manager
        _clientInfo.SubscribeOnStatusChanged(JoinedServerAsClient);
        _initializer.JoinServer();
    }

    public void BackButtonClicker()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Fires when application joins a server and gets a Client status confirmation.
    /// Sends PlayerJoined message to the host to request change of scene.
    /// </summary>
    /// <param name="status"> Status received from the server </param>
    private void JoinedServerAsClient(ushort status)
    {
        if (status == ClientStatus.Client)
        {
            // This message is handled in HostLobbyManager
            // In response - LoadLobby Message is sent
            // JoinedServerAsHost is handler in a different class due to the fact 
            // that the lobby doesnt exist while calling the method
            _connectionMenuMessageSender.SendPlayerJoinedMessage();
        }
        else
        {
            Debug.Log("Unable to join the server. Try again");
        }
        _clientInfo.UnsubscribeOnStatusChanged(JoinedServerAsClient);
    }

    /// <summary>
    /// Fires when application joins a server and gets a Host status confirmation.
    /// Sends PlayerJoined message to the host to request change of scene.
    /// If the host player cannot join the server - the server is closed and no game is initiated.
    /// </summary>
    /// <param name="status"> Status received from the server </param>
    private void JoinedServerAsHost(ushort status)
    {
        if (status == ClientStatus.Host)
        {
            // This message is handled in ConnectionMenuHostMessageReceiver
            // When joining, the Host is treaded exactly as other player with the exception of its LobbyState status.
            // Host's LobbyState status is added in LobbyInitializedHandler to prevent LobbyState from being a ProjectWide class
            _connectionMenuMessageSender.SendPlayerJoinedMessage();
        }
        else
        {
            Debug.Log("Server setup unsuccessful. Try again");
            _serverManager.CloseServer();
        }
        _clientInfo.UnsubscribeOnStatusChanged(JoinedServerAsHost);
    }

}