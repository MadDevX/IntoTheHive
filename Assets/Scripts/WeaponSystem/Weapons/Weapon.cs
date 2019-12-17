using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Weapon : IWeapon
{
    private Settings _settings;

    private List<IModule> _modules = new List<IModule>();
    public IFactory<ProjectileSpawnParameters, IProjectile[]> Factory { get; set; }

    private bool _wasSqueezed = false;

    public event Action<List<IModule>> OnWeaponRefreshed;

    public Weapon([Inject(Id = Identifiers.Ray)] IFactory<ProjectileSpawnParameters, ProjectileFacade[]> projectileFactory, Settings settings)
    {
        _settings = settings;
        Factory = projectileFactory;
    }

    public void ReleaseTrigger()
    {
        _wasSqueezed = false;
    }

    public void Reload()
    {
        Debug.Log("Reload!");
    }

    public bool Shoot(Vector2 position, float rotation, Vector2 offset)
    {
        if (_wasSqueezed == false)
        {
            _wasSqueezed = true;
            var spawnPos = position + offset.Rotate(rotation);
            Factory.Create(new ProjectileSpawnParameters(spawnPos, rotation, _settings.velocity, _settings.timeToLive, _modules));
        }
        return true;
    }

    public void AttachModule(IModule module)
    {
        ResetWeapon();
        _modules.Add(module);
        RefreshWeapon();
    }

    public void DetachModule(IModule module)
    {
        ResetWeapon();
        _modules.Remove(module);
        RefreshWeapon();
    }

    private void ResetWeapon()
    {
        for(int i = _modules.Count-1; i >= 0; i--)
        {
            _modules[i].DetachFromWeapon();
        }
    }

    private void RefreshWeapon()
    {
        _modules.Sort(CompareModules);
        for(int i = 0; i < _modules.Count; i++)
        {
            _modules[i].AttachToWeapon(this);
        }

        OnWeaponRefreshed?.Invoke(_modules);
    }

    private int CompareModules(IModule a, IModule b)
    {
        return a.Priority.CompareTo(b.Priority);
    }

    [System.Serializable]
    public class Settings
    {
        public float velocity;
        public float timeToLive;
    }
}
