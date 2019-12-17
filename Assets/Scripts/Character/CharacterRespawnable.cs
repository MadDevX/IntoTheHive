using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRespawnable : IRespawnable, IRespawner
{
    public event Action<CharacterSpawnParameters> OnSpawn;
    public event Action OnDespawn;

    public void Spawn(CharacterSpawnParameters parameters)
    {
        OnSpawn?.Invoke(parameters);
    }

    public void Despawn()
    {
        OnDespawn?.Invoke();
    }

}
