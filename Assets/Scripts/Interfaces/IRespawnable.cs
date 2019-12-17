using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRespawnable
{
    event Action<CharacterSpawnParameters> OnSpawn;
    event Action OnDespawn;
}

public interface IRespawner
{
    void Spawn(CharacterSpawnParameters parameters);
    void Despawn();
}