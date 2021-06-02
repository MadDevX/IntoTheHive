using DarkRift;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LobbyClientMessageReceiver: IInitializable, IDisposable
{
    private NetworkRelay _networkRelay;
    private LobbyStateManager _lobbyManager;

    private List<ushort> _connectedPlayers = new List<ushort>();

    public LobbyClientMessageReceiver(
        NetworkRelay networkRelay,
        LobbyStateManager lobbyManager
        )
    {
        _networkRelay = networkRelay;
        _lobbyManager = lobbyManager;
    }

    public void Initialize()
    {
        _networkRelay.Subscribe(Tags.UpdateLobby, ParseUpdateLobbyMessage);
    }

    public void Dispose()
    {
        _networkRelay.Unsubscribe(Tags.UpdateLobby, ParseUpdateLobbyMessage);
    }

    private void ParseUpdateLobbyMessage(Message message)
    {
        Debug.Log("Updated Lobby");
        _connectedPlayers.Clear();
        using (DarkRiftReader reader = message.GetReader())
        {
            while (reader.Position < reader.Length)
            {
                ushort id = reader.ReadUInt16();
                bool ready = reader.ReadBoolean();
                string nickname = reader.ReadString();
                _lobbyManager.AddPlayerToLobby(id,nickname,ready);
                _connectedPlayers.Add(id);
            }
            _lobbyManager.CheckDisconnectedPlayers(_connectedPlayers);
        }
    }

    
}

