using DarkRift;
using System;
using Zenject;

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

    // TODO MG : make some universal method to use in both classes
    // A similiar method is located in LobbyHostMessageReceiver. 
    // If you want to change this class make sure if those changes apply there also.
    private void HandleHostJoined(Message message)
    {

        ushort id;
        //string name;

        using (DarkRiftReader reader = message.GetReader())
        {
            id = reader.ReadUInt16();
            //name = reader.ReadString();
        }

        // Host is added to the LobbyState when his lobby is initialized
        // TODO MG : add some kind of sceneManager.GetSceneByName
        ushort sceneIndex = (ushort)2;

        _sender.SendLoadLobbyMessage(id, sceneIndex);
    }

}

