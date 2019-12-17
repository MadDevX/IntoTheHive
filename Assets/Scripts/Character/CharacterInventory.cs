using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class CharacterInventory : IItemContainer
{
    public ReadOnlyCollection<ItemInstance> Items { get; private set; }

    public event Action<ItemInstance> OnItemAdded;
    public event Action<ItemInstance> OnItemRemoved;

    private List<ItemInstance> _items = new List<ItemInstance>();
    public void AddItem(ItemInstance item)
    {
        _items.Add(item);
        UpdateItemsView();
        OnItemAdded?.Invoke(item);
    }

    public void RemoveItem(ItemInstance item)
    {
        if(_items.Remove(item))
        {
            UpdateItemsView();
            OnItemRemoved?.Invoke(item);
        }
    }

    private void UpdateItemsView()
    {
        Items = _items.AsReadOnly();
    }
}
