using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnassignedItems
{
    public event Action<ItemInstance> OnItemReassigned;

    public void RaiseReassigned(ItemInstance instance)
    {
        OnItemReassigned?.Invoke(instance);
    }
}
