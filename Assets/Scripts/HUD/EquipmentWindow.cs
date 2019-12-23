using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EquipmentWindow : ItemListWindow
{
    [Inject]
    public void Construct([Inject(Id = Identifiers.Equipment)] InventorySlot.Factory slotFactory)
    {
        _slotFactory = slotFactory;
    }
}
