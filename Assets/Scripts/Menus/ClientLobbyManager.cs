using System;
using UnityEngine.UI;
using Zenject;

public class ClientLobbyManager: IInitializable, IDisposable
{
    private Button _readyButton;
    private Button _leaveLobbyButton;

    public ClientLobbyManager(
        [Inject(Id = Identifiers.LobbyClientLeaveButton)]
        Button leaveButton,
        [Inject(Id = Identifiers.LobbyClientReadyButton)]
        Button readyButton)
    {
        _leaveLobbyButton = leaveButton;
        _readyButton = readyButton;
    }

    public void Initialize()
    {
        _readyButton.onClick.AddListener(SetReadyStatus);
        _leaveLobbyButton.onClick.AddListener(LeaveLobby);
    }

    public void Dispose()
    {
        _readyButton.onClick.RemoveListener(SetReadyStatus);
        _leaveLobbyButton.onClick.RemoveListener(LeaveLobby);
    }

    public void SetReadyStatus()
    {
        // Send Ready Message to the server
        throw new NotImplementedException();
    }

    public void LeaveLobby()
    {
        // disconnect from the server and change the scene to Connection Menu
        throw new NotImplementedException();
    }

}