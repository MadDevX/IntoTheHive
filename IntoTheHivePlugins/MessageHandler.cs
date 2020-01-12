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
            #region all clients
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

            if (message.Tag == Tags.LevelGraph)
                _plugin.BroadcastToAllClients(message, e);

            if (message.Tag == Tags.EndLevelTrigger)
                _plugin.BroadcastToAllClients(message, e);

            if (message.Tag == Tags.CloseDoorsMessage)
                _plugin.BroadcastToAllClients(message, e);

            if (message.Tag == Tags.OpenDoorsMessage)
                _plugin.BroadcastToAllClients(message, e);

            if (message.Tag == Tags.DisposeCharacter)
                _plugin.BroadcastToAllClients(message, e);

            if (message.Tag == Tags.SpawnPickup)
                _plugin.BroadcastToAllClients(message, e);

            if (message.Tag == Tags.DespawnPickup)
                _plugin.BroadcastToAllClients(message, e);

            if (message.Tag == Tags.AssignItem)
                _plugin.BroadcastToAllClients(message, e);

            if (message.Tag == Tags.WeaponChanged)
                _plugin.BroadcastToAllClients(message, e);

            if (message.Tag == Tags.UpdateGameState)
                _plugin.BroadcastToAllClients(message, e);
            #endregion

            #region other clients
            if (message.Tag == Tags.SpawnAI)
                _plugin.BroadcastToOtherClients(message, e);

            if (message.Tag == Tags.DespawnAI)
                _plugin.BroadcastToOtherClients(message, e);

            if (message.Tag == Tags.UpdateCharacterState)
                _plugin.BroadcastToOtherClients(message, e);

            if (message.Tag == Tags.UpdateCharacterEquipment)
                _plugin.BroadcastToOtherClients(message, e);

            if (message.Tag == Tags.DespawnCharacter)
                _plugin.BroadcastToOtherClients(message, e);

            if (message.Tag == Tags.TakeDamage)
                _plugin.BroadcastToOtherClients(message, e);

            if (message.Tag == Tags.UpdateHealth)
                _plugin.BroadcastToOtherClients(message, e);

            if (message.Tag == Tags.SpawnProjectile)
                _plugin.BroadcastToOtherClients(message, e);
            #endregion

            #region host only
            if (message.Tag == Tags.PlayerJoined)
                _plugin.BroadcastToHost(message, e);

            if (message.Tag == Tags.IsPlayerReady)
                _plugin.BroadcastToHost(message, e);

            if (message.Tag == Tags.RequestUpdateLobby)
                _plugin.BroadcastToHost(message, e);

            if (message.Tag == Tags.SceneReady)
                _plugin.BroadcastToHost(message, e);

            if (message.Tag == Tags.ClientReady)
                _plugin.BroadcastToHost(message, e);

            if (message.Tag == Tags.DeathRequest)
                _plugin.BroadcastToHost(message, e);

            if (message.Tag == Tags.RemoveItem)
                _plugin.BroadcastToHost(message, e);

            if (message.Tag == Tags.RequestSpawnPickup)
                _plugin.BroadcastToHost(message, e);
            #endregion

            if (message.Tag == Tags.LoadLobby)
            {
                using (DarkRiftReader reader = message.GetReader())
                {
                    ushort id = reader.ReadUInt16();
                    _plugin.BroadcastToClient(message, e,id);
                }
            }

            

        }
    }
}

