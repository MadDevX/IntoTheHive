﻿using GameLoop;
using System;
using UnityEngine;
using Zenject;

public struct CharacterSpawnParameters
{

}

public class CharacterFacade: MonoUpdatableObject, IPoolable<CharacterSpawnParameters, IMemoryPool>, IDisposable
{
    
    private IMemoryPool _pool;

    public CharacterFacade()
    {
        
    }

    public void Dispose()
    {
        if(_pool != null)
        _pool.Despawn(this);
    }

    public void OnDespawned()
    {
        _pool = null;
    }

    public void OnSpawned(CharacterSpawnParameters parameters, IMemoryPool pool)
    {
        _pool = pool;
    }

    public override void OnUpdate(float deltaTime)
    {
    }

    public class Factory: PlaceholderFactory<CharacterSpawnParameters, CharacterFacade>
    {
    }
}