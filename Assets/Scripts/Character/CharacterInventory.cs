using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : IItemContainer
{
    /// <summary>
    /// DO NOT MODIFY COLLECTION THROUGH THIS PROPERTY
    /// </summary>
    public List<ItemInstance> Items { get; } = new List<ItemInstance>();

    public event Action<ItemInstance> OnItemAdded;
    public event Action<ItemInstance> OnItemRemoved;

    public void AddItem(ItemInstance item)
    {
        Items.Add(item);
        OnItemAdded?.Invoke(item);
    }

    public void RemoveItem(ItemInstance item)
    {
        if(Items.Remove(item))
        {
            OnItemRemoved?.Invoke(item);
        }
    }
}
