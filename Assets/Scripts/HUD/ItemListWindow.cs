using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class ItemListWindow : MonoBehaviour
{
    [SerializeField] private bool _showEquippedItems;

    private PlayerRegistry _registry;
    private UnassignedItems _unassigned;
    protected InventorySlot.Factory _slotFactory;
    private List<InventorySlot> _slots = new List<InventorySlot>();

    [Inject]
    public void Construct(PlayerRegistry registry, UnassignedItems unassigned)
    {
        _registry = registry;
        _unassigned = unassigned;
    }

    private void Start()
    {
        _unassigned.OnItemReassigned += OnItemReassigned;
    }

    private void OnEnable()
    {
        InitItems();
    }

    private void OnDisable()
    {
        DisposeItems();
    }

    private void OnDestroy()
    {
        _unassigned.OnItemReassigned -= OnItemReassigned;
    }

    //TODO: add refreshing based on IItemContainer event
    private void InitItems()
    {
        foreach(var item in _registry.Player.Inventory.Items)
        {
            if (item.instance.IsEquipped == _showEquippedItems)
            {
                AddSlot(item);
            }
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
        if (obj.instance.IsEquipped != _showEquippedItems)
        {
            RemoveSlot(obj);
            _unassigned.RaiseReassigned(obj);
        }
    }

    private void OnItemReassigned(ItemInstance obj)
    {
        if (obj.instance.IsEquipped == _showEquippedItems)
        {
            AddSlot(obj);
        }
    }

    private void AddSlot(ItemInstance obj)
    {
        var slot = _slotFactory.Create(obj);
        _slots.Add(slot);
        InitSlot(slot);
    }

    private void RemoveSlot(ItemInstance obj)
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            if (_slots[i].Item == obj)
            {
                DisposeSlot(_slots[i]);
                _slots.RemoveAt(i);
            }
        }
    }
}
