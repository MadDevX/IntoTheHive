using DarkRift;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


namespace Networking.Character
{
    /// <summary>
    /// Used in local characters. Responsible for handling damage requests from the server.
    /// </summary>
    public class DamageReceiver : IInitializable, IDisposable
    {
        private NetworkRelay _networkRelay;
        private IDamageable _damageable;
        private CharacterInfo _info;

        public DamageReceiver(NetworkRelay networkRelay, IDamageable damageable, CharacterInfo info)
        {
            _networkRelay = networkRelay;
            _damageable = damageable;
            _info = info;
        }

        public void Initialize()
        {
            _networkRelay.Subscribe(Tags.TakeDamage, ParseMessage);
        }

        public void Dispose()
        {
            _networkRelay.Unsubscribe(Tags.TakeDamage, ParseMessage);
        }

        private void ParseMessage(Message message)
        {
            using (var reader = message.GetReader())
            {
                ushort characterId = reader.ReadUInt16();
                if(_info.Id == characterId && _info.IsLocal)
                {
                    float dmg = reader.ReadSingle();
                    _damageable.TakeDamage(dmg);
                }
            }
        }
    }
}
