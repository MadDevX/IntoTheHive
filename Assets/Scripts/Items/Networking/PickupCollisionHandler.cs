using DarkRift;
using Relays;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Networking.Items
{
    public class PickupCollisionHandler : IDisposable
    {
        private IRelay _relay;
        private ClientInfo _info;
        private NetId _netId;
        private ItemPickup _facade;

        public PickupCollisionHandler(IRelay relay, ClientInfo info, NetId netId, ItemPickup facade)
        {
            _relay = relay;
            _info = info;
            _netId = netId;
            _facade = facade;
            PreInitialize();
        }

        private void PreInitialize()
        {
            _relay.OnTrigger2DEnterEvt += HandleCollision;
        }

        public void Dispose()
        {
            _relay.OnTrigger2DEnterEvt -= HandleCollision;
        }

        private void HandleCollision(Collider2D obj)
        {
            var facade = obj.GetComponent<CharacterFacade>();
            if(facade != null && facade.CharacterType == CharacterType.Player)
            {
                //if host - send message to despawn and assign item to appropriate player
                if(_info.Status == ClientStatus.Host)
                {
                    SendDespawnMessage();
                    SendAssignItemMessage(facade);
                }
            }
        }

        private void SendAssignItemMessage(CharacterFacade facade)
        {
            using (var writer = DarkRiftWriter.Create())
            {
                writer.Write(facade.Id);
                writer.Write(_facade.Item.itemId);
                using (var message = Message.Create(Tags.AssignItem, writer))
                {
                    _info.Client.SendMessage(message, SendMode.Reliable);
                }
            }
        }

        private void SendDespawnMessage()
        {
            using (var writer = DarkRiftWriter.Create())
            {
                writer.Write(_netId.Id);
                using (var message = Message.Create(Tags.DespawnPickup, writer))
                {
                    _info.Client.SendMessage(message, SendMode.Reliable);
                }
            }
        }
    }
}
