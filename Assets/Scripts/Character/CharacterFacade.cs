using GameLoop;
using System;
using UnityEngine;
using Zenject;

public struct CharacterSpawnParameters
{
    public ushort Id;
    public float X;
    public float Y;
    public ushort SenderId;
    public bool IsLocal;
    public CharacterType CharacterType;
    public IHealth health;
    //What additional info should this contain?
}

public class CharacterFacade: MonoUpdatableObject, IPoolable<CharacterSpawnParameters, IMemoryPool>, IDisposable, IDamageable
{    
    public ushort Id;
    public CharacterType CharacterType { get; private set; }

    private IMemoryPool _pool;
    private IHealth _health;

    //[Inject]
    //public void Construct(IHealth health)
    //{
    //    _health = health;
    //}

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
        _health = parameters.health;
        _pool = pool;
    }

    public override void OnUpdate(float deltaTime)
    {
    }

    public float TakeDamage(float amount)
    {
        return _health.TakeDamage(amount);
    }

    public class Factory: PlaceholderFactory<CharacterSpawnParameters, CharacterFacade>
    {
    }    
}