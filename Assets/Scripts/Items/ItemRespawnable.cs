using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRespawnable : IRespawnable<ItemSpawnParameters>, IRespawner<ItemSpawnParameters>
{
    public event Action<ItemSpawnParameters> OnSpawn;
    public event Action OnDespawn;

    public void Spawn(ItemSpawnParameters parameters)
    {
        OnSpawn?.Invoke(parameters);
    }

    public void Despawn()
    {
        OnDespawn?.Invoke();
    }
}
