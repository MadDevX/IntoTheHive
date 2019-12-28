using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRespawnable : IRespawnable<PickupSpawnParameters>, IRespawner<PickupSpawnParameters>
{
    public event Action<PickupSpawnParameters> OnSpawn;
    public event Action OnDespawn;

    public void Spawn(PickupSpawnParameters parameters)
    {
        OnSpawn?.Invoke(parameters);
    }

    public void Despawn()
    {
        OnDespawn?.Invoke();
    }
}
