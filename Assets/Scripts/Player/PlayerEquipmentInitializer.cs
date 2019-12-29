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

    private List<IModule> _modulesBuffer = new List<IModule>();
    private List<ItemInstance> _itemsBuffer = new List<ItemInstance>();
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


        _modulesBuffer.Clear();
        _itemsBuffer.Clear();
        _itemsBuffer.AddRange(_inventory.Items);

        foreach(var id in obj.modules)
        {
            if (_itemsBuffer.Count == 0)
            {
                throw new ArgumentException("Requested unused module does not exist in inventory");
            }

            for (int i = 0; i < _itemsBuffer.Count; i++)
            {
                if (_itemsBuffer[i].data.itemId == id)
                {
                    var module = _itemsBuffer[i].instance as ModuleItem;
                    if (module != null)
                    {
                        _modulesBuffer.Add(module.Module);
                        _itemsBuffer.RemoveAt(i);
                        break;
                    }
                    else throw new ArgumentException("Item is not a module");
                }
                if (i == _inventory.Items.Count - 1)
                {
                    throw new ArgumentException("Requested unused module does not exist in inventory");
                }
            }
        }
        _weapon.SetModules(_modulesBuffer);
    }
}
