using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ItemPickup : MonoBehaviour, IPoolable<ItemSpawnParameters, IMemoryPool>, IDisposable
{
    [SerializeField] private SpriteRenderer _iconRenderer;

    public ItemInstance Item { get; private set; }
    private IMemoryPool _pool;
    private IRespawner<ItemSpawnParameters> _respawner;

    [Inject]
    public void Construct(IRespawner<ItemSpawnParameters> respawner)
    {
        _respawner = respawner;
    }

    public void OnSpawned(ItemSpawnParameters parameters, IMemoryPool pool)
    {
        _pool = pool;
        Item = parameters.item;
        transform.position = parameters.position;
        _iconRenderer.sprite = Item.data.icon;
        _respawner.Spawn(parameters);
    }

    public void OnDespawned()
    {
        _pool = null;
        _respawner.Despawn();
    }

    public void Dispose()
    {
        _pool?.Despawn(this);
    }

    public class Factory : PlaceholderFactory<ItemSpawnParameters, ItemPickup>
    {
    }
}
