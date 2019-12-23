﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InventoryWindow : ItemListWindow
{
    [Inject]
    public void Construct([Inject(Id = Identifiers.Inventory)] InventorySlot.Factory slotFactory)
    {
        _slotFactory = slotFactory;
    }
}
