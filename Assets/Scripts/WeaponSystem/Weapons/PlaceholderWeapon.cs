using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceholderWeapon : IWeapon
{
    private Projectile.Factory _projectileFactory;
    private Settings _settings;

    private bool _wasSqueezed = false;

    public PlaceholderWeapon(Projectile.Factory projectileFactory, Settings settings)
    {
        _projectileFactory = projectileFactory;
        _settings = settings;
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
            _projectileFactory.Create(new ProjectileSpawnParameters(spawnPos, rotation, _settings.velocity, _settings.timeToLive));
        }
        return true;
    }

    [System.Serializable]
    public class Settings
    {
        public float velocity;
        public float timeToLive;
    }
}
