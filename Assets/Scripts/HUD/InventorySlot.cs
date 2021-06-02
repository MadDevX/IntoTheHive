using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class InventorySlot : MonoBehaviour, IPoolable<ItemInstance, IMemoryPool>, IDisposable, IPointerClickHandler
{
    public event Action<ItemInstance> OnClick;
    public event Action<ItemInstance> OnRightClick;
    public ItemInstance Item { get; set; }
    [SerializeField] private Image _image;
    [SerializeField] private Image _background;
    [SerializeField] private Button _button;
    private IMemoryPool _pool;
    private Settings _settings;

    [Inject]
    public void Construct(Settings settings)
    {
        _settings = settings;
    }


    public void OnSpawned(ItemInstance item, IMemoryPool pool)
    {
        _pool = pool;
        Item = item;
        _image.sprite = item.data.icon;
        _button.onClick.AddListener(OnButtonClick);
        UpdateView();
    }

    public void Dispose()
    {
        _pool?.Despawn(this);
    }

    public void OnDespawned()
    {
        _pool = null;
        _button.onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        OnClick?.Invoke(Item);
        UpdateView();
    }

    private void UpdateView()
    {
        if(Item.instance.IsEquipped)
        {
            _background.color = _settings.activeColor;
        }
        else
        {
            _background.color = _settings.inactiveColor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick?.Invoke(Item);
        }
    }

    [System.Serializable]
    public class Settings
    {
        public Color activeColor;
        public Color inactiveColor;
    }

    public class Factory : PlaceholderFactory<ItemInstance, InventorySlot>
    {
    }
}
