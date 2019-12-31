using System;
using System.Collections;
using System.Collections.Generic;
using DarkRift;
using UnityEngine;
using Zenject;

public class LobbyRefresher : IInitializable, IDisposable
{
    private NetworkRelay _networkRelay;
    private LobbyMessageSender _lobbySender;
    private LobbyStateManager _stateManager;
    private ClientInfo _info;

    public LobbyRefresher(NetworkRelay networkRelay, LobbyMessageSender lobbySender, LobbyStateManager stateManager, ClientInfo info)
    {
        _networkRelay = networkRelay;
        _lobbySender = lobbySender;
        _stateManager = stateManager;
        _info = info;
    }

    public void Initialize()
    {
        _networkRelay.Subscribe(Tags.PlayerDisconnected, HandleDisconnected);
    }

    public void Dispose()
    {
        _networkRelay.Unsubscribe(Tags.PlayerDisconnected, HandleDisconnected);
    }

    public void RefreshLobby()
    {
        _lobbySender.SendUpdateLobbyMessage();
    }

    private void HandleDisconnected(Message obj)
    {
        if (_info.Status == ClientStatus.Host)
        {
            using (var reader = obj.GetReader())
            {
                var clientId = reader.ReadUInt16();
                _stateManager.RemovePlayerFromLobby(clientId);
                RefreshLobby();
            }
        }
    }
}
