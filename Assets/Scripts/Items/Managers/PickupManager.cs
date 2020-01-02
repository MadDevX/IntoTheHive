using DarkRift;
using DarkRift.Client.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PickupManager : IInitializable, IDisposable
{
    private ClientInfo _info;
    private NetworkRelay _networkRelay;
    private short _lastId;

    public PickupManager(ClientInfo info, NetworkRelay networkRelay)
    {
        _info = info;
        _networkRelay = networkRelay;

        _lastId = -1;
    }

    public void Initialize()
    {
        _networkRelay.Subscribe(Tags.RequestSpawnPickup, HandleRequest);
    }

    public void Dispose()
    {
        _networkRelay.Unsubscribe(Tags.RequestSpawnPickup, HandleRequest);
    }

    public void SpawnPickup(PickupSpawnRequestParameters parameters)
    {
        if (_info.Status == ClientStatus.Host)
        {
            SendSpawnMessage(parameters);
        }
        else if(_info.Status == ClientStatus.Client)
        {
            SendRequest(parameters);
        }
    }

    private void SendSpawnMessage(PickupSpawnRequestParameters parameters)
    {
        using (var writer = DarkRiftWriter.Create())
        {
            writer.Write(GetId());
            writer.Write(parameters.itemId);
            writer.Write(parameters.position.x);
            writer.Write(parameters.position.y);
            using (var message = Message.Create(Tags.SpawnPickup, writer))
            {
                _info.Client.SendMessage(message, SendMode.Reliable);
            }
        }
    }

    private void SendRequest(PickupSpawnRequestParameters parameters)
    {
        using (var writer = DarkRiftWriter.Create())
        {
            writer.Write(parameters.itemId);
            writer.Write(parameters.position.x);
            writer.Write(parameters.position.y);
            using (var message = Message.Create(Tags.RequestSpawnPickup, writer))
            {
                _info.Client.SendMessage(message, SendMode.Reliable);
            }
        }
    }

    private void HandleRequest(Message message)
    {
        using (var reader = message.GetReader())
        {
            var itemId = reader.ReadInt16();
            var x = reader.ReadSingle();
            var y = reader.ReadSingle();
            SpawnPickup(new PickupSpawnRequestParameters(itemId, new Vector2(x, y)));
        }
    }

    private short GetId()
    {
        _lastId++;
        return _lastId;
    }
}
