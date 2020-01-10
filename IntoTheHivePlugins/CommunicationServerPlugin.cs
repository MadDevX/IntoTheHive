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
            if(_clients.ContainsKey(e.Client.ID) == false)
            {
                _clients.Add(e.Client.ID, e.Client);
                if (_host == null)
                {
                    _host = e.Client;
                }

                using (DarkRiftWriter writer = DarkRiftWriter.Create())
                {
                    if (e.Client.ID == _host.ID)
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

                    using (Message playerDisconnectedBroadcast = Message.Create(Tags.PlayerDisconnected, playerDisconnected))
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
    }
}
