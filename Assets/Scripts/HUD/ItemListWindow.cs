using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class ItemListWindow : MonoBehaviour
{
    [SerializeField] private bool _showEquippedItems;

    protected PlayerRegistry _registry;
    private UnassignedItems _unassigned;
    protected InventorySlot.Factory _slotFactory;
    private List<InventorySlot> _slots = new List<InventorySlot>();

    [Inject]
    public void Construct(PlayerRegistry registry, UnassignedItems unassigned)
    {
        _registry = registry;
        _unassigned = unassigned;
        PreInitialize();
    }

    private void PreInitialize()
    {
        _unassigned.OnItemReassigned += OnItemReassigned;
        _unassigned.OnItemRemoved += RemoveSlot;
        _unassigned.OnPlayerUnset += DisposeItems;
    }

    private void OnDestroy()
    {
        _unassigned.OnItemReassigned -= OnItemReassigned;
        _unassigned.OnItemRemoved -= RemoveSlot;
        _unassigned.OnPlayerUnset -= DisposeItems;
    }

    private void DisposeItems()
    {
        foreach(var slot in _slots)
        {
            DisposeSlot(slot);
        }
        _slots.Clear();
    }

    /// <summary>
    /// Overrides should call base method at the beginning of their logic
    /// </summary>
    /// <param name="slot"></param>
    protected virtual void InitSlot(InventorySlot slot)
    {
        slot.OnClick += OnClick;
    }

    /// <summary>
    /// Overrides should call base method at the end of their logic
    /// </summary>
    /// <param name="slot"></param>
    protected virtual void DisposeSlot(InventorySlot slot)
    {
        slot.OnClick -= OnClick;
        slot.Dispose();
    }

    private void OnClick(ItemInstance obj)
    {
        obj.instance.UseItem(_registry.Player);
        _unassigned.RaiseReassigned(obj);
    }

    private void OnItemReassigned(ItemInstance item)
    {
        if (item.instance.IsEquipped == _showEquippedItems)
        {
            if (ContainsSlot(item) == false) AddSlot(item);
        }
        else
        {
            RemoveSlot(item);
        }
    }

    protected void AddSlot(ItemInstance obj)
    {
        var slot = _slotFactory.Create(obj);
        _slots.Add(slot);
        InitSlot(slot);
    }

    protected void RemoveSlot(ItemInstance obj)
    {
        for (int i = _slots.Count - 1; i >= 0; i--)
        {
            if (_slots[i].Item == obj)
            {
                DisposeSlot(_slots[i]);
                _slots.RemoveAt(i);
            }
        }
    }

    protected bool ContainsSlot(ItemInstance item)
    {
        foreach(var slot in _slots)
        {
            if (slot.Item == item) return true;
        }
        return false;
    }
}
