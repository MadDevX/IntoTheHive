using System;
using System.Collections;
using System.Collections.Generic;
using DarkRift;
using UnityEngine;

namespace Networking.Items
{
    public class DespawnMessageHandler : IDisposable
    {
        private NetworkRelay _networkRelay;
        private NetId _netId;
        private ItemPickup _facade;

        public DespawnMessageHandler(NetworkRelay networkRelay, NetId netId, ItemPickup facade)
        {
            _networkRelay = networkRelay;
            _netId = netId;
            _facade = facade;
            PreInitialize();
        }

        private void PreInitialize()
        {
            _networkRelay.Subscribe(Tags.DespawnPickup, HandleMessage);
        }

        public void Dispose()
        {
            _networkRelay.Unsubscribe(Tags.DespawnPickup, HandleMessage);
        }

        private void HandleMessage(Message message)
        {
            using (var reader = message.GetReader())
            {
                var pickupId = reader.ReadInt16();
                if (pickupId == _netId.Id)
                {
                    _facade.Dispose();
                }
            }
        }
    }
}