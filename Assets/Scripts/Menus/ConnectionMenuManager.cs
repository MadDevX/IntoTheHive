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
    private LobbyMessageSender _lobbyMessageSender;
    private NetworkedClientInitializer _initializer;
    private NetworkedSceneManager _networkedSceneManager;
    private ChangeSceneMessageSender _sceneMessageSender;
    private GlobalHostPlayerManager _globalHostPlayerManager;

    public ConnectionMenuManager(
        [Inject(Id = Identifiers.ConnetionMenuCreateServerButton)] Button serverButton,
        [Inject(Id = Identifiers.ConnetionMenuJoinServerButton)] Button joinButton,
        [Inject(Id = Identifiers.ConnetionMenuBackButton)] Button backButton,
        NetworkedClientInitializer connectionInitializer,
        GlobalHostPlayerManager globalHostPlayerManager,
        NetworkedSceneManager networkedSceneManager,
        ChangeSceneMessageSender sceneMessageSender,
        LobbyMessageSender lobbyMessageSender,
        ServerManager serverManager,
        ClientInfo clientInfo)
    {
        _joinButton = joinButton;
        _backButton = backButton;
        _serverButton = serverButton;

        _clientInfo = clientInfo;
        _serverManager = serverManager;
        _initializer = connectionInitializer;
        _lobbyMessageSender = lobbyMessageSender;
        _sceneMessageSender = sceneMessageSender;
        _networkedSceneManager = networkedSceneManager;
        _globalHostPlayerManager = globalHostPlayerManager;
    }

    public void Initialize()
    {
        _clientInfo.Status = ClientStatus.None;
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

    private void JoinedServerAsHost(ushort status)
    {
        if (status == ClientStatus.Host)
        {
            // TODO MG: send a message 
            //_sceneMessageSender.SendSceneChanged(2);
            _lobbyMessageSender.PlayerJoinedMessage();
        }
        else
        {
            Debug.Log("Server setup unsuccessful.Try again");
            _serverManager.CloseServer();
        }
        _clientInfo.StatusChanged -= JoinedServerAsHost;
    }

    private void JoinedServerAsClient(ushort status)
    {
        if (status == ClientStatus.Client)
        {
            //Handler to the response exists in Networked SceneManager
            _lobbyMessageSender.PlayerJoinedMessage();
            //TODO MG why is LobbyMessageSender in Connecton Menu?
            //TODO MG MOVE THIS TO ANOTHER CLASS OR RENAME THE CLASS
        }
        else
        {
            Debug.Log("Unable to join the server. Try again");
        }
        _clientInfo.StatusChanged -= JoinedServerAsClient;
    }


}