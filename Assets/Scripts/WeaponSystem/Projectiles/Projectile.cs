using GameLoop;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public struct ProjectileSpawnParameters
{
    public Vector2 position;
    public float rotation;
    public float velocity;
    public float ttl;
    public List<IModule> modules;

    public ProjectileSpawnParameters(Vector2 position, float rotation, float velocity, float ttl, List<IModule> modules)
    {
        this.position = position;
        this.rotation = rotation;
        this.velocity = velocity;
        this.ttl = ttl;
        this.modules = modules;
    }
}

public struct ProjectilePipelineParameters
{
    public Projectile projectile;
    public ProjectilePhysics physics;
    //TODO: collision information (CastHit)
    public ProjectilePipelineParameters(Projectile projectile, ProjectilePhysics physics)
    {
        this.projectile = projectile;
        this.physics = physics;
    }
}

public class Projectile : MonoBehaviour, IPoolable<ProjectileSpawnParameters, IMemoryPool>, IDisposable
{
    [SerializeField] private ProjectilePhysics _rb;
    [SerializeField] private TrailRenderer _trail;
    public PhasePipeline Pipeline { get; } = new PhasePipeline();
    public event Action<Projectile, RaycastHit2D, int> OnCollisionEnter
    {
        add
        {
            _rb.OnCollisionEnter += value;
        }
        remove
        {
            _rb.OnCollisionEnter -= value;
        }
    }
    private List<IModule> _currentModules = new List<IModule>();
    private IMemoryPool _pool;

    public Vector2 Position => _rb.Position;
    public int CollisionLimit { get; set; } = 0;

    public ProjectilePipelineParameters PipelineParameters = new ProjectilePipelineParameters();

    private void OnEnable()
    {
        DefaultProjectileModule.DecorateProjectile(this);
    }

    private void OnDisable()
    {
        DefaultProjectileModule.RemoveFromProjectile(this);
    }

    public void Dispose()
    {
        _pool.Despawn(this);
    }

    public void OnDespawned()
    {
        _pool = null;
        Pipeline.SetState(ProjectilePhases.Destroy, PipelineParameters);
        _rb.Velocity = Vector2.zero;
        DisposeModules();
    }

    public void OnSpawned(ProjectileSpawnParameters parameters, IMemoryPool pool)
    {
        _rb.Position = parameters.position;
        _rb.Velocity = Vector2.up.Rotate(parameters.rotation) * parameters.velocity;
        _rb.RemainingCollisions = CollisionLimit;
        _pool = pool;
        _trail.Clear();

        PipelineParameters.projectile = this;
        PipelineParameters.physics = _rb;

        if (parameters.modules != null)
        {
            _currentModules.Clear();
            _currentModules.AddRange(parameters.modules);
            InitModules();
        }
        Pipeline.SetState(ProjectilePhases.Created, PipelineParameters);
    }

    private void InitModules()
    {
        for (int i = 0; i < _currentModules.Count; i++)
        {
            _currentModules[i].DecorateProjectile(this);
        }
    }

    private void DisposeModules()
    {
        for (int i = _currentModules.Count - 1; i >= 0; i--)
        {
            _currentModules[i].RemoveFromProjectile(this);
        }
    }

    public class Factory : PlaceholderFactory<ProjectileSpawnParameters, Projectile>
    {
    }

    public class MultiFactory : MultiFactory<ProjectileSpawnParameters, Projectile>
    {
        public MultiFactory([Inject(Id = Identifiers.Bullet)] Factory factory) : base(factory){}
    }

    public class PhasePipeline : EventStateMachine<ProjectilePhases, ProjectilePipelineParameters>
    {
    }
}
