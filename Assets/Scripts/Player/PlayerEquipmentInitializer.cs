using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentInitializer : IDisposable
{
    private IRespawnable<CharacterSpawnParameters> _respawnable;
    private ItemFactory _itemFactory;
    private IItemContainer _inventory;
    private IWeapon _weapon;

    public PlayerEquipmentInitializer(IRespawnable<CharacterSpawnParameters> respawnable, ItemFactory itemFactory, IItemContainer inventory, IWeapon weapon)
    {
        _respawnable = respawnable;
        _itemFactory = itemFactory;
        _inventory = inventory;
        _weapon = weapon;
        PreInitialize();
    }

    private void PreInitialize()
    {
        _respawnable.OnSpawn += InitializeInventory;
    }

    public void Dispose()
    {
        _respawnable.OnSpawn -= InitializeInventory;
    }

    private void InitializeInventory(CharacterSpawnParameters obj)
    {
        foreach(var id in obj.items)
        {
            _inventory.AddItem(_itemFactory.Create(ItemType.Module, id));
        }

        var modules = new List<IModule>();
        foreach(var id in obj.modules)
        {
            for(int i = 0; i < _inventory.Items.Count; i++)
            {
                var item = _inventory.Items[i];
                if(item.data.itemId == id && item.instance.IsEquipped == false)
                {
                    var module = item.instance as ModuleItem;
                    if (module != null) modules.Add(module.Module);
                    else throw new ArgumentException("Item is not a module");
                }
                if (i == _inventory.Items.Count - 1)
                {
                    throw new ArgumentException("Requested unused module does not exist in inventory");
                }
            }
        }
        _weapon.SetModules(modules);
    }
}
