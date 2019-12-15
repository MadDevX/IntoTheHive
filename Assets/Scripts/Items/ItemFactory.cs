using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory
{
    private ModuleFactory _moduleFactory;

    public ItemFactory(ModuleFactory moduleFactory)
    {
        _moduleFactory = moduleFactory;
    }

    public IItem Create(ItemTypes type, short id)
    {
        switch(type)
        {
            case ItemTypes.Module:
                return new ModuleItem(_moduleFactory.Create(id));
            default:
                return null;
        }
    }
}
