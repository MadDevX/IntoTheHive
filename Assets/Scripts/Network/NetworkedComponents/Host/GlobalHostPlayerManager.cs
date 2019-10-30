using System;
using System.Collections.Generic;
using DarkRift;
using UnityEngine;
using Zenject;

//This class stores currently connected clients
public class GlobalHostPlayerManager: IInitializable, IDisposable
{
    public List<ushort> ConnectedPlayers;

    private NetworkRelay _relay;

    public GlobalHostPlayerManager(
        NetworkRelay relay)
    {
        _relay = relay;
        ConnectedPlayers = new List<ushort>();
    }

    public void Initialize()
    {
        _relay.Subscribe(Tags.PlayerJoined, HandlePlayerJoined);
        _relay.Subscribe(Tags.PlayerDisconnected, HandlePlayerDisconnected);
    }
    public void Dispose()
    {
        _relay.Unsubscribe(Tags.PlayerJoined, HandlePlayerJoined);
        _relay.Unsubscribe(Tags.PlayerDisconnected, HandlePlayerDisconnected);
    }

    private void HandlePlayerJoined(Message message)
    {
        Debug.Log("Player Joined");
        using (DarkRiftReader reader = message.GetReader())
        {
            ushort clientId;
            clientId = reader.ReadUInt16();
            ConnectedPlayers.Add(clientId);
        }
    }

    private void HandlePlayerDisconnected(Message message)
    {
        Debug.Log("Player disconnected");
        using (DarkRiftReader reader = message.GetReader())
        {
            ushort clientId;
            clientId = reader.ReadUInt16();
            ConnectedPlayers.Remove(clientId);
        }
    }
}

