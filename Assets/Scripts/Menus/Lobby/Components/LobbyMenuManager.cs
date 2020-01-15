using DarkRift.Client.Unity;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

/// <summary>
/// This class gathers button handlers from Lobby Menu 
/// It uses networking classes to set player status as ready and start the game
/// </summary>
public class LobbyMenuManager: IInitializable, IDisposable
{
    private bool _isPlayerReady;
    private Button _startGameButton;
    private Button _readyButton;
    private Button _leaveLobbyButton;
    
    private Text _startTextReady;
    private Text _startTextNotReady;

    private UnityClient _client;
    private ClientInfo _clientInfo;
    private ServerManager _serverManager;
    private LobbyMessageSender _messageSender;
    private LobbyStateManager _lobbyStateManager;
    private HostSceneManager _sceneManager;
    private IGameCycleController _cycleController;

    public LobbyMenuManager(
        [Inject(Id = Identifiers.LobbyStartGameButton)]
        Button startGameButton,
        [Inject(Id = Identifiers.LobbyReadyButton)]
        Button readyButton,
        [Inject(Id = Identifiers.LobbyLeaveButton)]
        Button leaveButton,
        [Inject(Id = Identifiers.StartTextReady)]
        Text startTextReady,
        [Inject(Id = Identifiers.StartTextNotReady)]
        Text startTextNotReady,
        UnityClient client,
        ClientInfo clientInfo,
        ServerManager serverManager,
        LobbyMessageSender messageSender,
        LobbyStateManager lobbyStateManager,
        HostSceneManager sceneManager,
        IGameCycleController cycleController)
    {
        _readyButton = readyButton;
        _leaveLobbyButton = leaveButton;
        _startGameButton = startGameButton;

        _startTextReady = startTextReady;
        _startTextNotReady = startTextNotReady;

        _client = client;
        _clientInfo = clientInfo;
        _sceneManager = sceneManager;
        _cycleController = cycleController;
        _serverManager = serverManager;
        _messageSender = messageSender;
        _lobbyStateManager = lobbyStateManager;
        
    }

    public void Initialize()
    {
        _startGameButton.onClick.AddListener(StartGame);
        _readyButton.onClick.AddListener(SetReadyStatus);
        _leaveLobbyButton.onClick.AddListener(LeaveLobby);

        // Disable non-host elements
        if (_clientInfo.Status != ClientStatus.Host)
        {
            DisableHostFunctionality();
        }

        SetupReadyHandlers();
    }

    public void Dispose()
    {
        _startGameButton.onClick.RemoveListener(StartGame);
        _readyButton.onClick.RemoveListener(SetReadyStatus);
        _leaveLobbyButton.onClick.RemoveListener(LeaveLobby);
        _lobbyStateManager.AllPlayersReadyChanged -= StartGameButtonSetActive;
    }  

    /// <summary>
    /// Starts the game by sending all players the ChangeScene message and spawning players when all clients are ready.
    /// </summary>
    public void StartGame()
    {
        _messageSender.SendStartGameMessage();
        _sceneManager.LoadNextLevel();
        _cycleController.RaiseOnGameStarted();
    }

    /// <summary>
    /// Sends a message to the host to update the client's ready status.
    /// </summary>
    public void SetReadyStatus()
    {
        _isPlayerReady = !_isPlayerReady;
        _messageSender.SendIsPlayerReadyMessage(_isPlayerReady);
    }

    public void LeaveLobby()
    {
        if (_clientInfo.Status == ClientStatus.Host)
        {
            _serverManager.CloseServer();
        }

        if (_clientInfo.Status == ClientStatus.Client)
        {
            _client.Disconnect();
        }

        SceneManager.LoadScene("ConnectionMenu");
    }

    /// <summary>
    /// This method disables start game button initially and makes it clickable when all players are active
    /// </summary>
    private void SetupReadyHandlers()
    {
        StartGameButtonSetActive(false);
        _lobbyStateManager.AllPlayersReadyChanged += StartGameButtonSetActive;
    }

    /// <summary>
    /// Enables StartGame button when all players ale ready. Otherwise disables it.
    /// </summary>
    /// <param name="isActive"></param>
    private void StartGameButtonSetActive(bool isActive)
    {
        _startGameButton.interactable = isActive;
        _startTextNotReady.enabled = !isActive;
        _startTextReady.enabled = isActive;
    }

    /// <summary>
    /// This fuction disables the host functionality existing in the lobby
    /// i.e. disables 'start game' button etc.
    /// </summary>
    private void DisableHostFunctionality()
    {
        _startGameButton.gameObject.SetActive(false);
    }

}