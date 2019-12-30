using DarkRift;
using System;
using UnityEngine;
using Zenject;

public class LobbyClientMessageReceiver: IInitializable, IDisposable
{
    private NetworkRelay _networkRelay;
    private PlayerEntryManager _entryManager;

    public LobbyClientMessageReceiver(
        NetworkRelay networkRelay,
        PlayerEntryManager entryManager
        )
    {
        _networkRelay = networkRelay;
        _entryManager = entryManager;
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
        using (DarkRiftReader reader = message.GetReader())
        {
            //TODO MG CHECKSIZE
            while (reader.Position < reader.Length)
            {
                ushort id = reader.ReadUInt16();
                bool ready = reader.ReadBoolean();
                string name;
                if (id == 0)
                    name = "Maciej";
                else
                    name = "Stefan";
                _entryManager.SetReady(name, id, ready);
                // TODO MG : update info on Lobby menu fields to show it to players
            }
        }
    }

    
}

