using System;
using System.Collections;
using System.Collections.Generic;
using DarkRift;
using UnityEngine;

namespace Networking.Character
{
    public class DisposeCharacterHandler : IDisposable
    {
        private NetworkRelay _networkRelay;
        private CharacterFacade _facade;

        public DisposeCharacterHandler(NetworkRelay networkRelay, CharacterFacade facade)
        {
            _networkRelay = networkRelay;
            _facade = facade;
            PreInitialize();
        }

        private void PreInitialize()
        {
            _networkRelay.Subscribe(Tags.DisposeCharacter, HandleMessage);
        }

        public void Dispose()
        {
            _networkRelay.Unsubscribe(Tags.DisposeCharacter, HandleMessage);
        }

        private void HandleMessage(Message message)
        {
            using (var reader = message.GetReader())
            {
                var charId = reader.ReadUInt16();
                if (_facade.Id == charId)
                {
                    _facade.Dispose();
                }
            }

        }
    }
}