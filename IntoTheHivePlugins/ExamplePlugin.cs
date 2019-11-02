using DarkRift;
using DarkRift.Server;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerPlugins
{
    public class ExamplePlugin : Plugin
    {
        private Dictionary<int, IClient> _clients;

        public override bool ThreadSafe => false;

        public override Version Version => new Version(1, 0, 0);

        public ExamplePlugin(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {
            _clients = new Dictionary<int, IClient>();

            ClientManager.ClientConnected += HandleClientConnected;
            ClientManager.ClientDisconnected += HandleClientDisconnected;
        }

        private void HandleClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            if(_clients.ContainsKey(e.Client.ID))
            {
                _clients.Remove(e.Client.ID);
                var clients = ClientManager.GetAllClients().Where(client => client != e.Client);

                using (DarkRiftWriter playerDisconnected = DarkRiftWriter.Create())
                {
                    playerDisconnected.Write(e.Client.ID);

                    using (Message platerDisconnectedBroadcast = Message.Create(Tags.DespawnCharacter, playerDisconnected))
                    {
                        foreach (IClient client in clients)
                        {
                            client.SendMessage(platerDisconnectedBroadcast, SendMode.Reliable);
                        }
                    }
                }
            }
        }

        private void HandleClientConnected(object sender, ClientConnectedEventArgs e)
        {
            
            // TODO MG change to send PlayerJoined message to the host
            // Spawn messages will be sent when the host will start the 
            // When changing the boolean value corresponding to wheter the the cleintId is locac should be removed
            // Separate message with client's own id could be sent separately
            _clients.Add(e.Client.ID, e.Client);

            //Write spawn message
            using (DarkRiftWriter newPlayerconnected = DarkRiftWriter.Create())
            {
                newPlayerconnected.Write(e.Client.ID);
                newPlayerconnected.Write(true);

                foreach(IClient client in _clients.Values)
                {
                    newPlayerconnected.Write(client.ID);
                    newPlayerconnected.Write(false);
                }

                using (Message newPlayerConnectedMessage = Message.Create(Tags.SpawnCharacter, newPlayerconnected))
                {
                    e.Client.SendMessage(newPlayerConnectedMessage, SendMode.Reliable);
                }
            }

            //Broadcast spawn to other players
            using (DarkRiftWriter newPlayerBroadcast = DarkRiftWriter.Create())
            {
                newPlayerBroadcast.Write(e.Client.ID);
                newPlayerBroadcast.Write(false);

                var clients = ClientManager.GetAllClients().Where(client => client != e.Client);
                
                using (Message newPlayerConnectedMessage = Message.Create(Tags.SpawnCharacter, newPlayerBroadcast))
                {
                    foreach (IClient client in clients)
                    {
                        client.SendMessage(newPlayerConnectedMessage, SendMode.Reliable);
                    }
                }                
            }


        }
    }
}
