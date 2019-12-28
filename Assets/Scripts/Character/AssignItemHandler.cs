using System;
using System.Collections;
using System.Collections.Generic;
using DarkRift;
using UnityEngine;

public class AssignItemHandler : IDisposable
{
    private NetworkRelay _networkRelay;
    private CharacterInfo _info;
    private ItemFactory _factory;
    private IItemContainer _inventory;

    public AssignItemHandler(NetworkRelay networkRelay, CharacterInfo info, ItemFactory factory, IItemContainer inventory)
    {
        _networkRelay = networkRelay;
        _info = info;
        _factory = factory;
        _inventory = inventory;
        PreInitialize();
    }

    private void PreInitialize()
    {
        _networkRelay.Subscribe(Tags.AssignItem, HandleMessage);
    }

    public void Dispose()
    {
        _networkRelay.Unsubscribe(Tags.AssignItem, HandleMessage);
    }

    private void HandleMessage(Message message)
    {
        using (var reader = message.GetReader())
        {
            var facadeId = reader.ReadUInt16();
            if(_info.Type == CharacterType.Player && _info.IsLocal && _info.Id == facadeId)
            {
                var itemId = reader.ReadInt16();
                var instance = _factory.Create(ItemType.Module, itemId); //TODO: handle other types of items
                _inventory.AddItem(instance);
            }
        }
    }


}
