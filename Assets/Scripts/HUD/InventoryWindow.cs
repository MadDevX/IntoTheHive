using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InventoryWindow : MonoBehaviour
{
    private PlayerRegistry _registry;
    private InventorySlot.Factory _slotFactory;
    private ItemFactory _itemFactory;
    private List<InventorySlot> _slots = new List<InventorySlot>();

    [Inject]
    public void Construct(PlayerRegistry registry, [Inject(Id = Identifiers.InventorySlot)] InventorySlot.Factory slotFactory, ItemFactory itemFactory)
    {
        _registry = registry;
        _slotFactory = slotFactory;
        _itemFactory = itemFactory;
    }

    private void OnEnable()
    {
        InitItems();
    }

    private void OnDisable()
    {
        DisposeItems();
    }

    //TODO: add refreshing based on IItemContainer event
    private void InitItems()
    {
        foreach(var item in _registry.Player.Inventory.Items)
        {
            var slot = _slotFactory.Create(item);
            _slots.Add(slot);
            InitSlot(slot);
        }
    }

    private void DisposeItems()
    {
        foreach(var slot in _slots)
        {
            DisposeSlot(slot);
        }
        _slots.Clear();
    }

    private void InitSlot(InventorySlot slot)
    {
        slot.OnClick += OnClick;
    }

    private void DisposeSlot(InventorySlot slot)
    {
        slot.OnClick -= OnClick;
        slot.Dispose();
    }

    private void OnClick(ItemInstance obj)
    {
        obj.instance.UseItem(_registry.Player);
    }
}
