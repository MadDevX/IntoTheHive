using DarkRift;
using System;
using UnityEngine.SceneManagement;
using Zenject;

/// <summary>
/// Scene-local connectionMenu only class used to handle messages as host on that scene
/// </summary>
public class ConnectionMenuHostMessageReceiver: IInitializable, IDisposable
{
    private ConnectionMenuMessageSender _sender;
    private LobbyState _state;
    private NetworkRelay _relay;

    public ConnectionMenuHostMessageReceiver(
        ConnectionMenuMessageSender sender,
        NetworkRelay relay)
    {
        _sender = sender;
        _relay = relay;
    }
  
    public void Initialize()
    {
        _relay.Subscribe(Tags.PlayerJoined, HandleHostJoined);
    }

    public void Dispose()
    {
        _relay.Unsubscribe(Tags.PlayerJoined, HandleHostJoined);
    }
    
    // A similiar method is located in LobbyHostMessageReceiver. 
    // If you want to change this class make sure if those changes apply there also.    
    private void HandleHostJoined(Message message)
    {
        // Host is added to the LobbyState in LobbyStateManager Initialize based on the connected players
        ushort id;
        //string name;
        // TODO MG CHECKSIZE
        using (DarkRiftReader reader = message.GetReader())
        {
            id = reader.ReadUInt16();
            string name = reader.ReadString();
        }
        
        ushort sceneIndex = 2;
        _sender.SendLoadLobbyMessage(id, sceneIndex);
    }

}

