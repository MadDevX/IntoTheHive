using System;
using System.Collections;
using System.Collections.Generic;
using DarkRift;
using UnityEngine;
using Zenject;

public class PlayerInventoryDataManager : IInitializable, IDisposable
{
    private NetworkRelay _networkRelay;
    private ItemDatabase _database;

    private Dictionary<ushort, List<ItemData>> _items = new Dictionary<ushort, List<ItemData>>();

    public PlayerInventoryDataManager(NetworkRelay networkRelay, ItemDatabase database)
    {
        _networkRelay = networkRelay;
        _database = database;
    }

    public void Initialize()
    {
        _networkRelay.Subscribe(Tags.AssignItem, AddItem);
        _networkRelay.Subscribe(Tags.RemoveItem, RemoveItem);
    }

    public void Dispose()
    {
        _networkRelay.Unsubscribe(Tags.AssignItem, AddItem);
        _networkRelay.Unsubscribe(Tags.RemoveItem, RemoveItem);
    }

    public List<ItemData> GetData(ushort id)
    {
        if(_items.TryGetValue(id, out var list))
        {
            return list;
        }
        else
        {
            return null;
        }
    }

    private void AddItem(Message message)
    {
        var info = ReadMessage(message);

        if (_items.TryGetValue(info.id, out var items))
        {
            items.Add(info.data);
        }
        else
        {
            var newItems = new List<ItemData>();
            newItems.Add(info.data);
            _items.Add(info.id, newItems);
        }
    }

    private void RemoveItem(Message message)
    {
        var info = ReadMessage(message);

        if (_items.TryGetValue(info.id, out var items))
        {
            items.Remove(info.data);
        }
    }

    private (ushort id, ItemData data) ReadMessage(Message message)
    {
        using (var reader = message.GetReader())
        {
            var facadeId = reader.ReadUInt16();
            var itemId = reader.ReadInt16();
            var data = _database.GetData(ItemType.Module, itemId);
            return (facadeId, data);
        }
    }

    private void ResetData()
    {
        _items.Clear();
    }
}
