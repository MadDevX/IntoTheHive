using DarkRift;
using DarkRift.Client.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager
{
    private ClientInfo _info;

    private short _lastId;

    public PickupManager(ClientInfo info)
    {
        _info = info;

        _lastId = -1;
    }

    public void SpawnPickup(ItemSpawnRequestParameters parameters)
    {
        if (_info.Status == ClientStatus.Host)
        {
            using (var writer = DarkRiftWriter.Create())
            {
                writer.Write(GetId());
                writer.Write(parameters.data.itemId);
                writer.Write(parameters.position.x);
                writer.Write(parameters.position.y);
                using (var message = Message.Create(Tags.SpawnPickup, writer))
                {
                    _info.Client.SendMessage(message, SendMode.Reliable);
                }
            }
        }
    }

    private short GetId()
    {
        _lastId++;
        return _lastId;
    }


}
