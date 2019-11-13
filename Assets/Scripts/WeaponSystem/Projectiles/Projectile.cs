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
    public Rigidbody2D rb;
    //TODO: collision information (CastHit)
    public ProjectilePipelineParameters(Projectile projectile, Rigidbody2D rb)
    {
        this.projectile = projectile;
        this.rb = rb;
    }
}

public class Projectile : MonoBehaviour, IPoolable<ProjectileSpawnParameters, IMemoryPool>, IDisposable
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private TrailRenderer _trail;
    [SerializeField] private Collider2D _collider;
    public event Action<Projectile, Collider2D, int> OnCollisionEnter;
    public PhasePipeline Pipeline { get; } = new PhasePipeline();

    private List<IModule> _currentModules = new List<IModule>();
    private IMemoryPool _pool;

    public Vector2 Position => _rb.position;
    public bool IsPiercing { get => _collider.isTrigger; set => _collider.isTrigger = value; }
    public int CollisionLimit { get; set; } = 0;
    private int _remainingCollisions;

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
        Pipeline.SetState(ProjectilePhases.Destroy, PipelineParameters);
        _pool.Despawn(this);
    }

    public void OnDespawned()
    {
        //DO NOT USE Factory.Create within OnDespawned because object is not fully returned to pool and but already can be used by next Create instruction
        _pool = null;
        _rb.velocity = Vector2.zero;
        DisposeModules();
    }

    public void OnSpawned(ProjectileSpawnParameters parameters, IMemoryPool pool)
    {
        transform.position = parameters.position;
        _rb.velocity = Vector2.up.Rotate(parameters.rotation) * parameters.velocity;
        _remainingCollisions = CollisionLimit;
        _pool = pool;
        _trail.Clear();

        PipelineParameters.projectile = this;
        PipelineParameters.rb = _rb;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _remainingCollisions--;
        OnCollisionEnter?.Invoke(this, collision.collider, _remainingCollisions);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _remainingCollisions--;
        OnCollisionEnter?.Invoke(this, collision, _remainingCollisions);
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
