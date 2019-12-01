using GameLoop;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RayProjectile : MonoUpdatableObject, IPoolable<ProjectileSpawnParameters, IMemoryPool>, IDisposable, IProjectile
{

    public ProjectilePhasePipeline Pipeline => new ProjectilePhasePipeline();

    public Vector2 Position { get; private set; }

    public Vector2 Velocity { get; set; }

    public float TravelTime => 0.0f;

    public float FixedTravelTime => 0.0f;

    public bool IsPiercing { get; set; } = true;
    public int CollisionLimit { get; set; } = 0;

    private int _remainingCollisions;
    private float _vfxTimer = 0.0f;

    public event Action<IProjectile, float> OnUpdateEvt;
    public event Action<IProjectile, float> OnFixedUpdateEvt;
    public event Action<IProjectile, Collider2D, int> OnCollisionEnter;

    private List<IModule> _currentModules = new List<IModule>();
    private IMemoryPool _pool;
    private ProjectilePipelineParameters _pipelineParameters = new ProjectilePipelineParameters();

    private int _layerMask;

    private Settings _settings;

    [Inject]
    public void Construct(Settings settings)
    {
        _settings = settings;
        _layerMask = Layers.Interactable.ToMask();
    }


    #region Spawn and Despawn

    public void OnSpawned(ProjectileSpawnParameters parameters, IMemoryPool pool)
    {
        _pool = pool;
        DefaultProjectileModule.DecorateProjectile(this);
        _remainingCollisions = CollisionLimit;
        _pipelineParameters.projectile = this;
        Velocity = Vector2.up.Rotate(parameters.rotation) * parameters.velocity;

        if(parameters.modules != null)
        {
            _currentModules.Clear();
            _currentModules.AddRange(parameters.modules);
            ProjectileUtility.InitModules(_currentModules, this);
        }

        ShootRay();
    }


    public void Dispose()
    {
        Pipeline.SetState(ProjectilePhases.Destroyed, _pipelineParameters);
        _pool.Despawn(this);
    }
    public void OnDespawned()
    {
        _pool = null;
        ProjectileUtility.DisposeModules(_currentModules, this);
        DefaultProjectileModule.RemoveFromProjectile(this);
    }

    #endregion

    private void ShootRay()
    {
        Pipeline.SetState(ProjectilePhases.Created, _pipelineParameters);
        var hits = Physics2D.RaycastAll(Position, Velocity, Velocity.magnitude * _settings.baseRange, _layerMask);
        ProcessHits(hits);
        //DrawRay();
    }

    public override void OnUpdate(float deltaTime)
    {
        if (_vfxTimer >= _settings.vfxDuration)
        {
            //DisableVfx();
        }
        _vfxTimer += deltaTime;
    }

    private void ProcessHits(RaycastHit2D[] hits)
    {
        for(int i = 0; i < hits.Length; i++)
        {
            _remainingCollisions--;
            OnCollisionEnter?.Invoke(this, hits[i].collider, _remainingCollisions);
        }
    }

    public void Destroy()
    {
        throw new NotImplementedException();
    }

    //private void DrawRay()
    //{
    //    _lineRenderer.SetPosition(0, Position);
    //    _lineRenderer.SetPosition(1, Position + Velocity * _settings.baseRange);
    //    _lineRenderer.enabled = true;
    //}

    //private void DisableVfx()
    //{
    //    _lineRenderer.enabled = false;
    //}

    [System.Serializable]
    public class Settings
    {
        public float baseRange;
        public float vfxDuration;
    }

    public class Factory : PlaceholderFactory<ProjectileSpawnParameters, RayProjectile>
    {
    }

    public class MultiFactory : MultiFactory<ProjectileSpawnParameters, RayProjectile>
    {
        public MultiFactory([Inject(Id = Identifiers.Bullet)] Factory factory) : base(factory){}
    }
}
