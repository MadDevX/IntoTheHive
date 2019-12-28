using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRespawnable<T>
{
    event Action<T> OnSpawn;
    event Action OnDespawn;
}

public interface IRespawner<T>
{
    void Spawn(T parameters);
    void Despawn();
}