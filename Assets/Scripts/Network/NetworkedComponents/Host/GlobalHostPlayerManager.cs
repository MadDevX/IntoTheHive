using System;
using System.Collections.Generic;
using DarkRift;
using UnityEngine;
using Zenject;

//This class stores currently connected clients
public class GlobalHostPlayerManager: IInitializable, IDisposable
{
    public List<ConnectedPlayerData> ConnectedPlayers = new List<ConnectedPlayerData>();

    private NetworkRelay _relay;

    public GlobalHostPlayerManager(
        NetworkRelay relay)
    {
        _relay = relay;
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
            string nickname;
            clientId = reader.ReadUInt16();
            nickname = reader.ReadString();
            if(ConnectedPlayers.Find(player => player.ID == clientId) == null)
            {
                ConnectedPlayers.Add(new ConnectedPlayerData(clientId,nickname));
            }
        }
    }

    private void HandlePlayerDisconnected(Message message)
    {
        Debug.Log("Player disconnected");
        using (DarkRiftReader reader = message.GetReader())
        {
            ushort clientId;
            clientId = reader.ReadUInt16();
            var disconnectedPlayer = ConnectedPlayers.Find(player => player.ID == clientId);
            ConnectedPlayers.Remove(disconnectedPlayer);
        }
    }
}

