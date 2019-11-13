using System;
using System.Collections.Generic;
using System.Text;
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
            if (message.Tag == Tags.UpdateCharacterState)
                _plugin.BroadcastToOtherClients(message, e);

            if (message.Tag == Tags.UpdateCharacterEquipment)
                _plugin.BroadcastToOtherClients(message, e);

            if (message.Tag == Tags.ChangeScene)
                _plugin.BroadcastToAllClients(message, e);

            if (message.Tag == Tags.SpawnCharacter)
                _plugin.BroadcastToAllClients(message, e);

            if (message.Tag == Tags.DespawnCharacter)
                _plugin.BroadcastToOtherClients(message, e);

            if (message.Tag == Tags.GameStarted)
                _plugin.BroadcastToAllClients(message, e);

            if (message.Tag == Tags.PlayerJoined)
                _plugin.BroadcastToOtherClients(message, e);

            if(message.Tag == Tags.RequestHostScene)
                _plugin.BroadcastToHost(message, e);

            if (message.Tag == Tags.ApplyHostScene)
            {
                using (DarkRiftReader reader = message.GetReader())
                {
                    //checksize
                    ushort id = reader.ReadUInt16();
                    _plugin.BroadcastToClient(message, e,id);
                }
            }
                

        }
    }
}

