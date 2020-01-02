using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public interface IItemContainer
{
    event Action<ItemInstance> OnItemAdded;
    event Action<ItemInstance> OnItemRemoved;
    event Action OnRefresh;

    ReadOnlyCollection<ItemInstance> Items { get; }

    void AddItem(ItemInstance item);
    void RemoveItem(ItemInstance item);

    void Refresh();
}
