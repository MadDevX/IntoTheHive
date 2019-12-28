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
                var data = _itemDatabase.GetData(type, id);
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
