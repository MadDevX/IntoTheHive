using DarkRift;
using DarkRift.Client.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Networking.Character
{
    public class DeathRequestSender : IDisposable
    {
        private IDamageable _damageable;
        private UnityClient _client;
        private CharacterInfo _info;

        public DeathRequestSender(IDamageable respawnable, UnityClient client, CharacterInfo info)
        {
            _damageable = respawnable;
            _client = client;
            _info = info;
            PreInitialize();
        }

        private void PreInitialize()
        {
            _damageable.OnDeath += SendRequest;
        }

        public void Dispose()
        {
            _damageable.OnDeath -= SendRequest;
        }

        private void SendRequest()
        {
            using (var writer = DarkRiftWriter.Create())
            {
                writer.Write(_info.Id);
                using (var msg = Message.Create(Tags.DeathRequest, writer))
                {
                    _client.SendMessage(msg, SendMode.Reliable);
                }
            }
        }
    }
}