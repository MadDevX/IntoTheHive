using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemContainer
{
    event Action<ItemInstance> OnItemAdded;
    event Action<ItemInstance> OnItemRemoved;

    /// <summary>
    /// Do not modify collection through this property
    /// </summary>
    List<ItemInstance> Items { get; }

    void AddItem(ItemInstance item);
    void RemoveItem(ItemInstance item);
}
