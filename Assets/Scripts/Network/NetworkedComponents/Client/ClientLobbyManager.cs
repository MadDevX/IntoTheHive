﻿using DarkRift;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

// TODO MG add to Lobby scene context
public class ClientLobbyManager: IInitializable, IDisposable
{
    private LobbyMessageSender _lobbyMessageSender;
    private NetworkRelay _networkRelay;

    public ClientLobbyManager(
        LobbyMessageSender lobbyMessageSender,
        NetworkRelay networkRelay)
    {
        _networkRelay = networkRelay;
        _lobbyMessageSender = lobbyMessageSender;
    }

    public void Initialize()
    {

        _networkRelay.Subscribe(Tags.UpdateLobby, ParseUpdateLobbyMessage);
    }

    public void Dispose()
    {
        _networkRelay.Unsubscribe(Tags.UpdateLobby, ParseUpdateLobbyMessage);
    }

    public void RequestLobbyUpdate()
    {
        // Now Fires also for host
        _lobbyMessageSender.SendRequestLobbyUpdate();
    }

    private void ParseUpdateLobbyMessage(Message message)
    {
        Debug.Log("Updated");
        using (DarkRiftReader reader = message.GetReader())
        {
            //checksize
            while(reader.Position < reader.Length)
            {
                ushort id = reader.ReadUInt16();
                bool ready = reader.ReadBoolean();
                // TODO MG : update info on Lobby menu fields to show it to players
            }
        }
    }

    
}

