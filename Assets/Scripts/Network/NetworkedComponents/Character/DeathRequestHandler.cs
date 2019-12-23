using DarkRift;
using DarkRift.Client.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Networking.Character
{
    /// <summary>
    /// Executes only on host side. Handles death requests and informs all clients about despawned character
    /// </summary>
    public class DeathRequestHandler : IDisposable
    {
        private NetworkRelay _networkRelay;
        private UnityClient _client;
        private ClientInfo _connectionInfo;
        private CharacterInfo _info;
        private CharacterFacade _facade;

        public DeathRequestHandler(NetworkRelay networkRelay, UnityClient client, ClientInfo connectionInfo, CharacterInfo info, CharacterFacade facade)
        {
            _networkRelay = networkRelay;
            _client = client;
            _connectionInfo = connectionInfo;
            _info = info;
            _facade = facade;
            PreInitialize();
        }

        private void PreInitialize()
        {
            _networkRelay.Subscribe(Tags.DeathRequest, HandleMessage);
        }

        public void Dispose()
        {
            _networkRelay.Unsubscribe(Tags.DeathRequest, HandleMessage);
        }

        private void HandleMessage(Message message)
        {
            if (_connectionInfo.Status == ClientStatus.Host)
            {
                using (var reader = message.GetReader())
                {
                    var characterId = reader.ReadUInt16();
                    ushort localId = _info.Id;
                    
                    if (localId == characterId)
                    {
                        Debug.LogWarning("Character dead received. ID = " + localId);
                        Debug.LogWarning("message content = " + characterId);
                        using (var writer = DarkRiftWriter.Create())
                        {
                            writer.Write(characterId);
                            using (var msg = Message.Create(Tags.DisposeCharacter, writer))
                            {
                                _client.SendMessage(msg, SendMode.Reliable);
                            }
                        }
                    }
                }
            }

        }
    }
}
