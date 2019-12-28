using GameLoop;
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
    public IHealth health;
    //What additional info should this contain?
}

public class CharacterFacade: MonoBehaviour, IPoolable<CharacterSpawnParameters, IMemoryPool>, IDisposable, IHealth, IDamageable
{
    //TODO: remove this
    public List<ItemData> itemsToInitialize;

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
    public CharacterType CharacterType { get; set; } //TODO: forward CharacterInfo property
    
    public float MaxHealth => _health.MaxHealth;
    public float Health => _health.Health;
    public IItemContainer Inventory { get; private set; }
    public IWeapon Weapon { get; private set; }

    private IMemoryPool _pool;
    private IHealth _health;
    private IDamageable _damageable;
    private IRespawner<CharacterSpawnParameters> _respawner;

    [Inject]
    public void Construct(IHealth health, IDamageable damageable, IRespawner<CharacterSpawnParameters> respawner, IItemContainer itemContainer, IWeapon weapon)
    {
        _health = health;
        _damageable = damageable;
        _respawner = respawner;
        Inventory = itemContainer;
        Weapon = weapon;
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
        //_health = parameters.health; TODO: read health data from spawn parameters
        _pool = pool;
        _respawner.Spawn(parameters);
    }

    public float TakeDamage(float amount) => _damageable.TakeDamage(amount);

    public class Factory: PlaceholderFactory<CharacterSpawnParameters, CharacterFacade>
    {
    }    
}