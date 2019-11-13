using DarkRift.Client.Unity;
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
    private NetworkedSceneManager _networkedSceneManager;
    private ChangeSceneMessageSender _sceneMessageSender;
    private NetworkedClientInitializer _initializer;

    public ConnectionMenuManager(
        [Inject(Id = Identifiers.ConnetionMenuCreateServerButton)] Button serverButton,
        [Inject(Id = Identifiers.ConnetionMenuJoinServerButton)] Button joinButton,
        [Inject(Id = Identifiers.ConnetionMenuBackButton)] Button backButton,
        NetworkedClientInitializer connectionInitializer,
        NetworkedSceneManager networkedSceneManager,
        ChangeSceneMessageSender sceneMessageSender,
        ServerManager serverManager,
        ClientInfo clientInfo)
    {
        _serverButton = serverButton;
        _joinButton = joinButton;
        _backButton = backButton;

        _clientInfo = clientInfo;
        _serverManager = serverManager;
        _sceneMessageSender = sceneMessageSender;
        _networkedSceneManager = networkedSceneManager;
        _initializer = connectionInitializer;
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
        _clientInfo.statusChanged += JoinedServerAsHost;
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
        _clientInfo.statusChanged += JoinedServerAsClient;
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
            _sceneMessageSender.SendSceneChanged(2);
        }
        else
        {
            Debug.Log("Server setup unsuccessful.Try again");
            _serverManager.CloseServer();
        }
        _clientInfo.statusChanged -= JoinedServerAsHost;
    }

    private void JoinedServerAsClient(ushort status)
    {
        if (status == ClientStatus.Client)
        {
            //Handler to the response exists in Networked SceneManager
            _sceneMessageSender.RequestHostScene();
        }
        else
        {
            Debug.Log("Unable to join the server. Try again");
        }
        _clientInfo.statusChanged -= JoinedServerAsClient;
    }


}