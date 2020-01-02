using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InventoryWindow : ItemListWindow
{
    private PickupManager _pickupManager;

    [Inject]
    public void Construct(
        [Inject(Id = Identifiers.Inventory)] InventorySlot.Factory slotFactory, 
        PickupManager pickupManager)
    {
        _slotFactory = slotFactory;
        _pickupManager = pickupManager;
    }

    protected override void InitSlot(InventorySlot slot)
    {
        slot.OnRightClick += HandleRightClick;
        base.InitSlot(slot);
    }

    protected override void DisposeSlot(InventorySlot slot)
    {
        slot.OnRightClick -= HandleRightClick;
        base.DisposeSlot(slot);
    }

    private void HandleRightClick(ItemInstance item)
    {
        _registry.Player.Inventory.RemoveItem(item);
        RemoveSlot(item);
        _pickupManager.SpawnPickup(new PickupSpawnRequestParameters(item.data.itemId, _registry.Player.Position + Vector2.up.Rotate(_registry.Player.Rotation) * 1.5f));
    }


}
