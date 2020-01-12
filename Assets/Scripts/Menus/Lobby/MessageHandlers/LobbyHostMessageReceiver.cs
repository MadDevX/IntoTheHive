using System;
using DarkRift;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

/// <summary>
/// This class handles message receival for host side of the lobby
/// </summary>
public class LobbyHostMessageReceiver: IInitializable, IDisposable
{    
    private NetworkRelay _relay;
    private LobbyStateManager _lobbyStateManager;
    private LobbyMessageSender _lobbyMessageSender;
    public LobbyHostMessageReceiver(
        NetworkRelay relay,
        LobbyStateManager lobbyStateManager,
        LobbyMessageSender lobbyMessageSender
        )
    {
        _relay = relay;
        _lobbyStateManager = lobbyStateManager;
        _lobbyMessageSender = lobbyMessageSender;
    }

    public void Initialize()
    {
        _relay.Subscribe(Tags.PlayerJoined, HandlePlayerJoined);
        _relay.Subscribe(Tags.RequestUpdateLobby, HandleRequestUpdateLobby);
        _relay.Subscribe(Tags.IsPlayerReady, HandleIsPlayerReady);
    }

    public void Dispose()
    {
        _relay.Unsubscribe(Tags.PlayerJoined, HandlePlayerJoined);
        _relay.Unsubscribe(Tags.RequestUpdateLobby, HandleRequestUpdateLobby);
        _relay.Unsubscribe(Tags.IsPlayerReady, HandleIsPlayerReady);
    }

    private void HandleIsPlayerReady(Message message)
    {
        ushort id;
        bool isReady;

        //TODO MG CHECKSIZE
        using (DarkRiftReader reader = message.GetReader())
        {
            id = reader.ReadUInt16();
            isReady = reader.ReadBoolean();
        }

        _lobbyStateManager.SetReady(id, isReady);
        _lobbyMessageSender.SendUpdateLobbyMessage();
    }
    
    private void HandlePlayerJoined(Message message)
    {
        ushort id;
        string name;

        //TODO MG CHECKSIZE
        using (DarkRiftReader reader = message.GetReader())
        {
            id = reader.ReadUInt16();
            name = reader.ReadString();
        }

        _lobbyStateManager.AddPlayerToLobby(id,name);
        ushort sceneIndex = (ushort)2;
        _lobbyMessageSender.SendLoadLobbyMessage(id,sceneIndex);
    }

    private void HandleRequestUpdateLobby(Message message)
    {
        _lobbyMessageSender.SendUpdateLobbyMessage();
    }
}

