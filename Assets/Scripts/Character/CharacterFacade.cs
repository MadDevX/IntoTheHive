﻿using GameLoop;
using System;
using UnityEngine;
using Zenject;

public struct CharacterSpawnParameters
{
    public ushort Id;
    public ushort SenderId;
    public bool IsLocal;
    public CharacterType CharacterType;
    //What additional info should this contain?
}

public class CharacterFacade: MonoUpdatableObject, IPoolable<CharacterSpawnParameters, IMemoryPool>, IDisposable
{    
    private IMemoryPool _pool;
    public ushort Id;
    public CharacterType CharacterType { get; private set; }

    public CharacterFacade()
    {
        // To be filled
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
        Id = parameters.Id;
        CharacterType = parameters.CharacterType;
        _pool = pool;
    }

    public override void OnUpdate(float deltaTime)
    {
    }

    public class Factory: PlaceholderFactory<CharacterSpawnParameters, CharacterFacade>
    {
    }
}