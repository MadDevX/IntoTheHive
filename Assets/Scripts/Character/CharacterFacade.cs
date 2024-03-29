﻿using GameLoop;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public struct CharacterSpawnParameters
{
    public ushort Id;
    public float X;
    public float Y;
    //public ushort playerId;
    public bool IsLocal;
    public CharacterType CharacterType;
    public List<short> items;
    public List<short> modules;
    //What additional info should this contain?
}

public class CharacterFacade: MonoBehaviour, IPoolable<CharacterSpawnParameters, IMemoryPool>, IDisposable, IHealth, IDamageable
{
    public event Action<DamageTakenArgs> OnDamageTaken
    {
        add
        {
            _damageable.OnDamageTaken += value;
        }
        remove
        {
            _damageable.OnDamageTaken -= value;
        }
    }

    public event Action<DeathParameters> OnDeath
    {
        add
        {
            _damageable.OnDeath += value;
        }
        remove
        {
            _damageable.OnDeath -= value;
        }
    }

    public ushort Id;
    public CharacterType CharacterType { get; set; } 
    
    public float MaxHealth => _health.MaxHealth;
    public float Health => _health.Health;
    public IItemContainer Inventory { get; private set; }
    public IWeapon Weapon { get; private set; }

    public Vector2 Position => _rb.position;
    public float Rotation => _rb.rotation;

    private IMemoryPool _pool;
    private IHealth _health;
    private IDamageable _damageable;
    private IRespawner<CharacterSpawnParameters> _respawner;
    private Rigidbody2D _rb;

    [Inject]
    public void Construct(
        IHealth health, 
        IDamageable damageable, 
        IRespawner<CharacterSpawnParameters> respawner, 
        IItemContainer itemContainer,
        IWeapon weapon,
        Rigidbody2D rb)
    {
        _health = health;
        _damageable = damageable;
        _respawner = respawner;
        Inventory = itemContainer;
        Weapon = weapon;
        _rb = rb;
    }

    public void Dispose()
    {
        if(_pool != null)
        _pool.Despawn(this);
    }

    public void OnDespawned()
    {
        _pool = null;
        _respawner.Despawn();
    }

    public void OnSpawned(CharacterSpawnParameters parameters, IMemoryPool pool)
    {
        Id = parameters.Id;
        CharacterType = parameters.CharacterType;
        _pool = pool;
        _respawner.Spawn(parameters);
    }

    public float TakeDamage(float amount) => _damageable.TakeDamage(amount);

    public class Factory: PlaceholderFactory<CharacterSpawnParameters, CharacterFacade>
    {
    }    
}