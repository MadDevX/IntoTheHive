using System;
using UnityEngine.UI;
using Zenject;

public class HostLobbyManager: IInitializable, IDisposable
{
    private Button _startGameButton;
    private Button _readyButton;
    private Button _leaveLobbyButton;

    public HostLobbyManager(
        [Inject(Id = Identifiers.LobbyStartGameButton)]
        Button startGameButton,
        [Inject(Id = Identifiers.LobbyHostReadyButton)]
        Button readyButton,
        [Inject(Id = Identifiers.LobbyHostLeaveButton)]
        Button leaveButton)
    {
        _startGameButton = startGameButton;
        _readyButton = readyButton;
        _leaveLobbyButton = leaveButton;
    }

    public void Initialize()
    {
        _startGameButton.onClick.AddListener(StartGame);
        _readyButton.onClick.AddListener(SetReadyStatus);
        _leaveLobbyButton.onClick.AddListener(LeaveLobby);
    }

    public void Dispose()
    {
        _startGameButton.onClick.RemoveListener(StartGame);
        _readyButton.onClick.RemoveListener(SetReadyStatus);
        _leaveLobbyButton.onClick.RemoveListener(LeaveLobby);
    }

    public void StartGame()
    {
        throw new NotImplementedException();
    }

    public void SetReadyStatus()
    {
        throw new NotImplementedException();
    }

    public void LeaveLobby()
    {
        // Send Info that the server is closing
        // Go Back to menu 
        // Close server
        throw new NotImplementedException();
    }
}