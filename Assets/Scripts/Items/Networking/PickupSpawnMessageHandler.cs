using System;
using System.Collections;
using System.Collections.Generic;
using DarkRift;
using UnityEngine;
using Zenject;

namespace Networking.Items
{
    public class PickupSpawnMessageHandler : IInitializable, IDisposable
    {
        private NetworkRelay _networkRelay;
        private ItemDatabase _itemDatabase;
        private ItemPickup.Factory _pickupFactory;

        public PickupSpawnMessageHandler(NetworkRelay networkRelay, ItemDatabase itemDatabase, [Inject(Id = Identifiers.Inventory)] ItemPickup.Factory pickupFactory)
        {
            _networkRelay = networkRelay;
            _itemDatabase = itemDatabase;
            _pickupFactory = pickupFactory;
        }

        public void Initialize()
        {
            _networkRelay.Subscribe(Tags.SpawnPickup, HandleMessage);
        }

        public void Dispose()
        {
            _networkRelay.Unsubscribe(Tags.SpawnPickup, HandleMessage);
        }

        private void HandleMessage(Message obj)
        {
            using (var reader = obj.GetReader())
            {
                var entityId = reader.ReadInt16();
                var itemId = reader.ReadInt16();
                var xPos = reader.ReadSingle();
                var yPos = reader.ReadSingle();
                var parameters = new PickupSpawnParameters(entityId, _itemDatabase.GetData(ItemType.Module, itemId), new Vector2(xPos, yPos));
                _pickupFactory.Create(parameters);
            }
        }
    }
}
