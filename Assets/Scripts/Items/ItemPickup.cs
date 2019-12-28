using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ItemPickup : MonoBehaviour, IPoolable<PickupSpawnParameters, IMemoryPool>, IDisposable
{
    [SerializeField] private SpriteRenderer _iconRenderer;

    public ItemData Item { get; private set; }
    private IMemoryPool _pool;
    private IRespawner<PickupSpawnParameters> _respawner;

    [Inject]
    public void Construct(IRespawner<PickupSpawnParameters> respawner)
    {
        _respawner = respawner;
    }

    public void OnSpawned(PickupSpawnParameters parameters, IMemoryPool pool)
    {
        _pool = pool;
        Item = parameters.item;
        transform.position = parameters.position;
        _iconRenderer.sprite = Item.icon;
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

    public class Factory : PlaceholderFactory<PickupSpawnParameters, ItemPickup>
    {
    }
}
