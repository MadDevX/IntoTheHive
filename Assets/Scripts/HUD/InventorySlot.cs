using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class InventorySlot : MonoBehaviour, IPoolable<ItemInstance, IMemoryPool>, IDisposable
{
    public event Action<ItemInstance> OnClick;
    public ItemInstance Item { get; set; }
    [SerializeField] private Image _image;
    [SerializeField] private Button _button;
    private IMemoryPool _pool;

    public void OnSpawned(ItemInstance item, IMemoryPool pool)
    {
        _pool = pool;
        Item = item;
        _image.sprite = item.data.icon;
        _button.onClick.AddListener(OnButtonClick);
    }

    public void Dispose()
    {
        if(_pool != null)
        {
            _pool.Despawn(this);
        }
    }

    public void OnDespawned()
    {
        _pool = null;
        _button.onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        OnClick?.Invoke(Item);
    }

    public class Factory : PlaceholderFactory<ItemInstance, InventorySlot>
    {
    }
}
