using DarkRift;
using DarkRift.Server;
using ServerPlugins;

public class MessageHandler
{
    private CommunicationServerPlugin _plugin;

    public MessageHandler(CommunicationServerPlugin plugin)
    {
        _plugin = plugin;
    }

    public void Client_MessageReceived(object sender, MessageReceivedEventArgs e)
    {
        using (Message message = e.GetMessage())
        {
            if (message.Tag == Tags.ChangeScene)
                _plugin.BroadcastToAllClients(message, e);

            if (message.Tag == Tags.ChangeSceneWithReply)
                _plugin.BroadcastToAllClients(message, e);            

            if (message.Tag == Tags.GameStarted)
                _plugin.BroadcastToAllClients(message, e);

            if (message.Tag == Tags.UpdateLobby)
                _plugin.BroadcastToAllClients(message, e);

            if (message.Tag == Tags.SpawnCharacter)
                _plugin.BroadcastToAllClients(message, e);

            if (message.Tag == Tags.UpdateCharacterState)
                _plugin.BroadcastToOtherClients(message, e);

            if (message.Tag == Tags.UpdateCharacterEquipment)
                _plugin.BroadcastToOtherClients(message, e);

            if (message.Tag == Tags.DespawnCharacter)
                _plugin.BroadcastToOtherClients(message, e);

            if(message.Tag == Tags.PlayerJoined)
                _plugin.BroadcastToHost(message, e);

            if (message.Tag == Tags.IsPlayerReady)
                _plugin.BroadcastToHost(message, e);

            if (message.Tag == Tags.RequestUpdateLobby)
                _plugin.BroadcastToHost(message, e);

            if (message.Tag == Tags.SceneReady)
                _plugin.BroadcastToHost(message, e);

            if (message.Tag == Tags.LoadLobby)
            {
                using (DarkRiftReader reader = message.GetReader())
                {
                    //checksize
                    //TODO change to sombe kind of load scene
                    ushort id = reader.ReadUInt16();
                    _plugin.BroadcastToClient(message, e,id);
                }
            }

            if (message.Tag == Tags.LevelGraph)
                _plugin.BroadcastToAllClients(message, e);

            if (message.Tag == Tags.ClientReady)
                _plugin.BroadcastToHost(message, e);
        }
    }
}

