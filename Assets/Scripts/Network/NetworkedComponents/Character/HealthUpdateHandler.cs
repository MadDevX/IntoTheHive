using System;
using System.Collections;
using System.Collections.Generic;
using DarkRift;
using UnityEngine;
using Zenject;

namespace Networking.Character
{
    /// <summary>
    /// Used in networked characters. Responsible for updating health of assigned character based on network messages.
    /// </summary>
    public class HealthUpdateHandler : IInitializable, IDisposable
    {
        private IHealthSetter _health;
        private NetworkRelay _networkRelay;
        private CharacterInfo _info;

        public HealthUpdateHandler(IHealthSetter health, NetworkRelay networkRelay, CharacterInfo info)
        {
            _health = health;
            _networkRelay = networkRelay;
            _info = info;
        }


        public void Initialize()
        {
            _networkRelay.Subscribe(Tags.UpdateHealth, HandleMessage);
        }

        public void Dispose()
        {
            _networkRelay.Unsubscribe(Tags.UpdateHealth, HandleMessage);
        }

        private void HandleMessage(Message msg)
        {
            using(var reader = msg.GetReader())
            {
                var charId = reader.ReadUInt16();
                if(_info.Id == charId)
                {
                    var damage = reader.ReadSingle();
                    var health = reader.ReadSingle();
                    _health.Health = health;
                }
            }
        }
    }
}