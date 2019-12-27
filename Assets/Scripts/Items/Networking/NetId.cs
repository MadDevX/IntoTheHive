using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Networking.Items
{
    public class NetId : IDisposable
    {
        public short Id { get; private set; }

        private IRespawnable<ItemSpawnParameters> _respawnable;

        public NetId(IRespawnable<ItemSpawnParameters> respawnable)
        {
            _respawnable = respawnable;
            PreInitialize();
        }

        private void PreInitialize()
        {
            _respawnable.OnSpawn += OnSpawn;
            _respawnable.OnDespawn += OnDespawn;
        }

        public void Dispose()
        {
            _respawnable.OnSpawn -= OnSpawn;
            _respawnable.OnDespawn -= OnDespawn;
        }

        private void OnSpawn(ItemSpawnParameters obj)
        {
            Id = obj.id;
        }

        private void OnDespawn()
        {
            Id = -1;
        }
    }
}