using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class ItemFactory
{
    private ModuleFactory _moduleFactory;
    private ItemDatabase _itemDatabase;

    public ItemFactory(ModuleFactory moduleFactory, ItemDatabase itemDatabase)
    {
        _moduleFactory = moduleFactory;
        _itemDatabase = itemDatabase;
    }

    public ItemInstance Create(ItemType type, short id)
    {
        switch(type)
        {
            case ItemType.Module:
                var data = GetData(type, id);
                return new ItemInstance(new ModuleItem(_moduleFactory.Create(id)), data);
            default:
                return null;
        }
    }

    public ItemInstance Create(ItemData data)
    {
        switch(data.type)
        {
            case ItemType.Module:
                return new ItemInstance(new ModuleItem(_moduleFactory.Create(data.itemId)), data);
            default:
                return null;
        }
    }

    private ItemData GetData(ItemType type, short id)
    {
        foreach(var data in _itemDatabase.dataList)
        {
            if(data.itemId == id && data.type == type)
            {
                return data;
            }
        }
        throw new ArgumentException($"Tried to get data that does not exist. Type: {type} | ID: {id}");
    }


    public ReadOnlyCollection<short> GetIds(ItemType type)
    {
        switch(type)
        {
            case ItemType.Module:
                return _moduleFactory.Ids;
            default:
                return null;
        }
    }
}
