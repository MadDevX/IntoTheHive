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
    public ushort playerId;
    public bool IsLocal;
    public CharacterType CharacterType;
    public IHealth health;
    //What additional info should this contain?
}

public class CharacterFacade: MonoBehaviour, IPoolable<CharacterSpawnParameters, IMemoryPool>, IDisposable, IHealth
{
    //TODO: remove this
    public List<ItemData> itemsToInitialize;

    public event Action<float> OnDamageTaken;
    public event Action OnDeath;

    public ushort Id;
    public CharacterType CharacterType { get; private set; } //TODO: forward CharacterInfo property

    public float MaxHealth => _health.MaxHealth;
    public float Health => _health.Health;

    public IItemContainer Inventory { get; private set; }
    public IWeapon Weapon { get; private set; }

    private IMemoryPool _pool;
    private IHealth _health;
    private IRespawner _respawner;
    private ItemFactory _factory;

    [Inject]
    public void Construct(IHealth health, IRespawner respawner, IItemContainer itemContainer, IWeapon weapon, ItemFactory factory)
    {
        _health = health;
        _respawner = respawner;
        Inventory = itemContainer;
        Weapon = weapon;
        _factory = factory;
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


        foreach (var item in itemsToInitialize)
        {
            var instance = item.CreateItem(_factory);
            Inventory.AddItem(instance);
        }
    }

    public float TakeDamage(float amount)
    {
        var ret = _health.TakeDamage(amount);
        OnDamageTaken?.Invoke(amount);
        return ret;
    }

    public class Factory: PlaceholderFactory<CharacterSpawnParameters, CharacterFacade>
    {
    }    
}