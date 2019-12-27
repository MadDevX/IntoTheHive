using System;
using DarkRift;
using DarkRift.Client.Unity;
using UnityEngine;
using Zenject;

/// <summary>
/// Receives messages from hostDoorManager and calls functions from door manager.
/// </summary>
public class ClientDoorManager: IInitializable, IDisposable
{
    private NetworkRelay _networkRelay;
    private DoorManager _doorManager;
    private GenericMessageWithResponseClient _sender;

    public ClientDoorManager(
        NetworkRelay relay,
        DoorManager doorManager,
        GenericMessageWithResponseClient sender)
    {
        _networkRelay = relay;
        _doorManager = doorManager;
        _sender = sender;
    }

    public void Initialize()
    {
        _networkRelay.Subscribe(Tags.OpenDoorsMessage, HandleOpenDoors);
        _networkRelay.Subscribe(Tags.CloseDoorsMessage, HandleCloseDoors);
    }

    public void Dispose()
    {
        _networkRelay.Unsubscribe(Tags.OpenDoorsMessage, HandleOpenDoors);
        _networkRelay.Unsubscribe(Tags.CloseDoorsMessage, HandleCloseDoors);
    }

    private void HandleCloseDoors(Message message)
    {        
        using(DarkRiftReader reader = message.GetReader())
        {
            //TODO MG CHECKSIZE
            ushort roomId = reader.ReadUInt16();
            _doorManager.CloseAllDoorsInRoom(roomId);
            _sender.SendClientReady();
        }
    }

    private void HandleOpenDoors(Message message)
    {
        using (DarkRiftReader reader = message.GetReader())
        {
            //TODO MG CHECKSIZE
            ushort roomId = reader.ReadUInt16();
            _doorManager.OpenAllDoorsInRoom(roomId);
            _sender.SendClientReady();
        }
    }

}