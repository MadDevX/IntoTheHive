using DarkRift;
using DarkRift.Server;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerPlugins
{
    public class CommunicationServerPlugin : Plugin
    {
        public override bool ThreadSafe => false;

        public override Version Version => new Version(1, 0, 0);

        private Dictionary<ushort, IClient> _clients;
        private MessageHandler _messageHandler;
        private IClient _host;


        public CommunicationServerPlugin(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {
            _clients = new Dictionary<ushort, IClient>();
            _messageHandler = new MessageHandler(this);

            ClientManager.ClientConnected += HandleClientConnected;
            ClientManager.ClientDisconnected += HandleClientDisconnected;
        }

        private void HandleClientConnected(object sender, ClientConnectedEventArgs e)
        {
            e.Client.MessageReceived += _messageHandler.Client_MessageReceived;
            _clients.Add(e.Client.ID, e.Client);
            if (_host == null)
            {
                _host = e.Client;
            }

            // Send PlayerJoined message to host?
            using (DarkRiftWriter writer = DarkRiftWriter.Create())
            {
                if(e.Client.ID == _host.ID)
                {
                    writer.Write(ClientStatus.Host);
                }
                else
                {
                    writer.Write(ClientStatus.Client);
                }

                using (Message message = Message.Create(Tags.ConnectionInfo, writer))
                {
                    
                    e.Client.SendMessage(message, SendMode.Reliable);
                }
            }
        }


        private void HandleClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            if (_clients.ContainsKey(e.Client.ID))
            {
                _clients.Remove(e.Client.ID);
                e.Client.MessageReceived -= _messageHandler.Client_MessageReceived;
                var clients = ClientManager.GetAllClients().Where(client => client != e.Client);

                using (DarkRiftWriter playerDisconnected = DarkRiftWriter.Create())
                {
                    playerDisconnected.Write(e.Client.ID);

                    using (Message playerDisconnectedBroadcast = Message.Create(Tags.DespawnCharacter, playerDisconnected))
                    {
                        foreach (IClient client in clients)
                        {
                            client.SendMessage(playerDisconnectedBroadcast, SendMode.Reliable);
                        }
                    }
                }
            }
        }

        public void BroadcastToAllClients(Message message, MessageReceivedEventArgs e)
        {
            foreach (IClient client in ClientManager.GetAllClients())
            {
                client.SendMessage(message, e.SendMode);
            }
        }

        public void BroadcastToHost(Message message, MessageReceivedEventArgs e)
        {
            _host.SendMessage(message, e.SendMode);
        }

        public void BroadcastToClient(Message message, MessageReceivedEventArgs e, ushort id)
        {
            IClient client;
            _clients.TryGetValue(id, out client);

            if(client != null)
            {
                client.SendMessage(message, e.SendMode);
            }
        }

        public void BroadcastToOtherClients(Message message, MessageReceivedEventArgs e)
        {
            var clients = ClientManager.GetAllClients().Where(client => client != e.Client);
            foreach (IClient client in clients)
            {
                client.SendMessage(message, e.SendMode);
            }
        }

        //public void BroadcastSpawnMessages(Message message, MessageReceivedEventArgs e)
        //{
        //    List<ushort> playersToSpawn = new List<ushort>();
        //    using (DarkRiftReader reader = message.GetReader())
        //    {
        //        //checksize
        //        while(reader.Position < reader.Length)
        //        {
        //            playersToSpawn.Add(reader.ReadUInt16());
        //        }
        //    }

        //    foreach(IClient client in ClientManager.GetAllClients())
        //    {
        //        client.SendMessage(message, SendMode.Reliable);
        //    }

        //}

        //private void SendSpawnMesssages(object sender, ClientDisconnectedEventArgs e)
        //{
        //    // TODO MG change to send PlayerJoined message to the host
        //    // Spawn messages will be sent when the host will start the 
        //    // When changing the boolean value corresponding to wheter the the cleintId is locac should be removed
        //    // Separate message with client's own id could be sent separately

        //    //Write spawn message
        //    using (DarkRiftWriter spawnWriter = DarkRiftWriter.Create())
        //    {
        //        spawnWriter.Write(e.Client.ID);
        //        spawnWriter.Write(true);

        //        foreach (IClient client in ClientManager.GetAllClients())
        //        {
        //            spawnWriter.Write(client.ID);
        //            spawnWriter.Write(false);
        //        }

        //        using (Message newPlayerConnectedMessage = Message.Create(Tags.SpawnCharacter, spawnWriter))
        //        {
        //            e.Client.SendMessage(newPlayerConnectedMessage, SendMode.Reliable);
        //        }
        //    }

        //    //Broadcast spawn to other players
        //    using (DarkRiftWriter spawnPlayerBroadcast = DarkRiftWriter.Create())
        //    {
        //        spawnPlayerBroadcast.Write(e.Client.ID);
        //        spawnPlayerBroadcast.Write(false);

        //        var clients = ClientManager.GetAllClients().Where(client => client != e.Client);

        //        using (Message newPlayerConnectedMessage = Message.Create(Tags.SpawnCharacter, spawnPlayerBroadcast))
        //        {
        //            foreach (IClient client in clients)
        //            {
        //                client.SendMessage(newPlayerConnectedMessage, SendMode.Reliable);
        //            }
        //        }
        //    }
        //}


    }
}
