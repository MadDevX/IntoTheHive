using DarkRift;
using DarkRift.Client.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


namespace Networking.Character
{
    /// <summary>
    /// Used in local characters. Responsible for informing server about health of assigned character.
    /// </summary>
    public class HealthUpdateSender : IInitializable, IDisposable
    {
        private UnityClient _client;
        private IDamageable _damageable;
        private IHealth _health;
        private CharacterInfo _info;

        public HealthUpdateSender(UnityClient client, IDamageable damageable, IHealth health, CharacterInfo info)
        {
            _client = client;
            _damageable = damageable;
            _health = health;
            _info = info;
        }


        public void Initialize()
        {
            _damageable.OnDamageTaken += SendMessage;
        }

        public void Dispose()
        {
            _damageable.OnDamageTaken -= SendMessage;
        }

        private void SendMessage(DamageTakenArgs args)
        {
            using (var writer = DarkRiftWriter.Create())
            {
                writer.Write(_info.Id);
                writer.Write(args.damage);
                writer.Write(_health.Health);

                using (var msg = Message.Create(Tags.UpdateHealth, writer))
                {
                    _client.SendMessage(msg, SendMode.Reliable);
                }
            }
        }
    }
}