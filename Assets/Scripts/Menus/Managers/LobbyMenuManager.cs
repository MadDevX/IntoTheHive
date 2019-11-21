using DarkRift.Client.Unity;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class LobbyMenuManager: IInitializable, IDisposable
{
    private Button _startGameButton;
    private Button _readyButton;
    private Button _leaveLobbyButton;

    private UnityClient _client;
    private ClientInfo _clientInfo;
    private ServerManager _serverManager;
    private HostManager _hostManager;

    public LobbyMenuManager(
        [Inject(Id = Identifiers.LobbyStartGameButton)]
        Button startGameButton,
        [Inject(Id = Identifiers.LobbyReadyButton)]
        Button readyButton,
        [Inject(Id = Identifiers.LobbyLeaveButton)]
        Button leaveButton,
        UnityClient client,
        ClientInfo clientInfo,
        HostManager hostManager,
        ServerManager serverManager
        )
    {
        _startGameButton = startGameButton;
        _readyButton = readyButton;
        _leaveLobbyButton = leaveButton;

        _client = client;
        _clientInfo = clientInfo;
        _serverManager = serverManager;
        _hostManager = hostManager;
    }

    public void Initialize()
    {
        _startGameButton.onClick.AddListener(StartGame);
        _readyButton.onClick.AddListener(SetReadyStatus);
        _leaveLobbyButton.onClick.AddListener(LeaveLobby);

        if(_clientInfo.Status != ClientStatus.Host)
        {
            DisableHostFunctionality();
        }
    }

    public void Dispose()
    {
        _startGameButton.onClick.RemoveListener(StartGame);
        _readyButton.onClick.RemoveListener(SetReadyStatus);
        _leaveLobbyButton.onClick.RemoveListener(LeaveLobby);
    }

    public void StartGame()
    {
        _hostManager.LoadNextLevel();
    }

    public void SetReadyStatus()
    {
        //_playerManager.SetReady();
    }

    public void LeaveLobby()
    {
        if (_clientInfo.Status == ClientStatus.Host)
        {
            // TODO MG : Send Info that the server is closing???
            _serverManager.CloseServer();
            // Should this also disconnect the host client or should he react to "disconnected event"?
        }

        if (_clientInfo.Status == ClientStatus.Client)
        {
            _client.Disconnect();    
        }

        SceneManager.LoadScene("ConnectionMenu");
    }

    private void DisableHostFunctionality()
    {
        _startGameButton.gameObject.SetActive(false);
    }


}