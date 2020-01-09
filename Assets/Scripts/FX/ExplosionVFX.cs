using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ExplosionVFX : MonoBehaviour, IPoolable<Vector3, IMemoryPool>, IDisposable
{
    [SerializeField] private Animator _anim;

    private IMemoryPool _pool;
    private Settings _settings;
    private int _hash;

    [Inject]
    public void Construct(Settings settings)
    {
        _settings = settings;
        _hash = Animator.StringToHash(_settings.animationTriggerName);
    }

    public void OnSpawned(Vector3 position, IMemoryPool pool)
    {
        _pool = pool;
        transform.position = position;
        _anim.SetTrigger(_hash);
    }

    //this dispose should be called at the end of animation clip by an animation event
    public void Dispose()
    {
        _pool.Despawn(this);
    }

    public void OnDespawned()
    {
        _pool = null;
    }

    public class Factory : PlaceholderFactory<Vector3, ExplosionVFX>
    {
    }

    [System.Serializable]
    public class Settings
    {
        public string animationTriggerName;
    }
}
