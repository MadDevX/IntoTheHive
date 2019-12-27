using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILootCreator : IDisposable
{
    private IRespawnable<CharacterSpawnParameters> _respawnable;
    private PickupManager _pickupManager;
    private ItemDatabase _database;
    private Rigidbody2D _rb;

    public AILootCreator(IRespawnable<CharacterSpawnParameters> respawnable, PickupManager pickupManager, ItemDatabase database, Rigidbody2D rb)
    {
        _respawnable = respawnable;
        _pickupManager = pickupManager;
        _database = database;
        _rb = rb;
        PreInitialize();
    }

    private void PreInitialize()
    {
        _respawnable.OnDespawn += OnDespawn;
    }

    public void Dispose()
    {
        _respawnable.OnDespawn -= OnDespawn;
    }

    private void OnDespawn()
    {
        var index = UnityEngine.Random.Range(0, _database.dataList.Count - 1);
        var data = _database.dataList[index];
        _pickupManager.SpawnPickup(new ItemSpawnRequestParameters(data, _rb.position));
    }

}
