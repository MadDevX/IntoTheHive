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
    private Settings _settings;
    public AILootCreator(IRespawnable<CharacterSpawnParameters> respawnable, PickupManager pickupManager, ItemDatabase database, Rigidbody2D rb, Settings settings)
    {        
        _respawnable = respawnable;
        _pickupManager = pickupManager;
        _database = database;
        _rb = rb;
        _settings = settings;
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
        float generatedNumber = UnityEngine.Random.Range(0f, 1f);
        if(generatedNumber<= _settings.lootDropChance)
        {
            var index = UnityEngine.Random.Range(0, _database.dataList.Count);
            var data = _database.dataList[index];
            _pickupManager.SpawnPickup(new PickupSpawnRequestParameters(data.itemId, _rb.position));
        }
    }

    [System.Serializable] 
    public class Settings
    {
        public float lootDropChance = 0.3f;
    }

}
