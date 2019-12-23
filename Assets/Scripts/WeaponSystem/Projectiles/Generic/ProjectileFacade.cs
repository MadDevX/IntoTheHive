using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProjectileFacade : MonoBehaviour, IPoolable<ProjectileSpawnParameters, IMemoryPool>, IDisposable, IProjectile
{
    private ProjectileInitializer _initializer;
    private IProjectilePosition _position;
    private IProjectileVelocity _velocity;
    private IProjectileTime _time;
    private IProjectileFixedTime _fixedTime;
    private IProjectileCollision _collision;
    private IProjectileDummy _dummy;
    private ProjectileDestroyAfterCollision _destroyCollision;
    private IMemoryPool _pool;

    public ProjectilePhasePipeline Pipeline { get; private set; }

    public Vector2 Position { get => _position.Position; }

    public Vector2 Velocity { get => _velocity.Velocity; set => _velocity.Velocity = value; }

    public bool IsDummy => _dummy.IsDummy;

    public float TravelTime => _time.TravelTime;

    public float FixedTravelTime => _fixedTime.FixedTravelTime;

    public bool IsPiercing { get => _collision.IsPiercing; set => _collision.IsPiercing = value; }

    public int CollisionLimit { get => _destroyCollision.CollisionLimit; set => _destroyCollision.CollisionLimit = value; }

    public event Action<IProjectile, float> OnUpdateEvt;
    public event Action<IProjectile, float> OnFixedUpdateEvt;
    public event Action<IProjectile, Collider2D, int> OnCollisionEnter;

    private ProjectilePipelineParameters _parameters;

    [Inject]
    public void Construct(ProjectilePhasePipeline pipeline,
        ProjectileInitializer initializer,
        IProjectilePosition position, 
        IProjectileVelocity velocity, 
        IProjectileTime time, 
        IProjectileFixedTime fixedTime,
        IProjectileCollision collision,
        IProjectileDummy dummy,
        ProjectileDestroyAfterCollision destroyCollision)
    {
        Pipeline = pipeline;
        _initializer = initializer;
        _position = position;
        _velocity = velocity;
        _time = time;
        _fixedTime = fixedTime;
        _collision = collision;
        _dummy = dummy;
        _destroyCollision = destroyCollision;
        _parameters = new ProjectilePipelineParameters(this);
    }


    public void OnSpawned(ProjectileSpawnParameters parameters, IMemoryPool pool)
    {
        AttachUpdates();
        _pool = pool;
        _initializer.CreateProjectile(parameters);
    }
    public void Destroy()
    {
        if (Pipeline.State != ProjectilePhases.Destroyed)
        {
            Dispose();
        }
    }

    public void Dispose()
    {
        DetachUpdates();
        _initializer.DespawnProjectile();
        _pool.Despawn(this);
    }

    public void OnDespawned()
    {
    }


    #region UPDATE FORWARDING
    private void AttachUpdates()
    {
        _time.OnUpdateEvt += OnUpdate;
        _fixedTime.OnFixedUpdateEvt += OnFixedUpdate;
    }

    private void OnUpdate(float deltaTime)
    {
        OnUpdateEvt?.Invoke(this, deltaTime);
    }

    private void OnFixedUpdate(float deltaTime)
    {
        OnFixedUpdateEvt?.Invoke(this, deltaTime);
    }

    private void DetachUpdates()
    {
        _time.OnUpdateEvt -= OnUpdate;
        _fixedTime.OnFixedUpdateEvt -= OnFixedUpdate;
    }
    #endregion

    public class Factory : PlaceholderFactory<ProjectileSpawnParameters, ProjectileFacade>
    {
    }
}
