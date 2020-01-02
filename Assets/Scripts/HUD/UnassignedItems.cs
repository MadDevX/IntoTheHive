using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UnassignedItems : IDisposable
{
    private PlayerRegistry _registry;

    public UnassignedItems(PlayerRegistry registry)
    {
        _registry = registry;
        PreInitialize();
    }

    /// <summary>
    /// Invoked when item is added or used 
    /// </summary>
    public event Action<ItemInstance> OnItemReassigned;

    /// <summary>
    /// Invoked when item is removed
    /// </summary>
    public event Action<ItemInstance> OnItemRemoved;

    /// <summary>
    /// Invoked when current player is replaced by new one
    /// </summary>
    public event Action OnPlayerUnset;

    private void PreInitialize()
    {
        _registry.OnPlayerSet += OnSet;
        _registry.OnPlayerUnset += OnUnset;
    }

    public void Dispose()
    {
        _registry.OnPlayerSet -= OnSet;
        _registry.OnPlayerUnset -= OnUnset;
    }

    private void OnSet(CharacterFacade facade)
    {
        facade.Inventory.OnItemAdded += RaiseReassigned;
        facade.Inventory.OnItemRemoved += RaiseRemoved;
        facade.Inventory.OnRefresh += ScanItems;
        ScanItems();
    }

    private void OnUnset(CharacterFacade facade)
    {
        facade.Inventory.OnItemAdded -= RaiseReassigned;
        facade.Inventory.OnItemRemoved -= RaiseRemoved;
        facade.Inventory.OnRefresh -= ScanItems;
        OnPlayerUnset?.Invoke();
    }

    public void RaiseReassigned(ItemInstance instance)
    {
        OnItemReassigned?.Invoke(instance);
    }

    private void RaiseRemoved(ItemInstance instance)
    {
        OnItemRemoved?.Invoke(instance);
    }

    private void ScanItems()
    {
        foreach(var item in _registry.Player.Inventory.Items)
        {
            OnItemReassigned?.Invoke(item);
        }
    }
}
