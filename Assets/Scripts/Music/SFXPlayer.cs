using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Music
{
    class SFXPlayer : IDisposable
    {
        private ProjectileInitializer _projectileInitializer;
        private ISoundProvider _soundProvider;
        private IProjectileCollision _projectileCollision;
        public SFXPlayer(ISoundProvider soundProvider, ProjectileInitializer projectileInitializer, IProjectileCollision projectileCollision)
        {
            _projectileInitializer = projectileInitializer;
            _soundProvider = soundProvider;
            _projectileCollision = projectileCollision;
            PreInitialize();
        }

        private void PreInitialize()
        {
            _projectileInitializer.OnProjectileInitialized += OnShoot;
            _projectileCollision.OnCollisionEnter += OnHit;
        }

        public void Dispose()
        {
            _projectileInitializer.OnProjectileInitialized -= OnShoot;
            _projectileCollision.OnCollisionEnter -= OnHit;
        }


        private void OnHit(Collider2D collider2D)
        {
            _soundProvider.PlayHitSound();
        }

        private void OnShoot(ProjectileSpawnParameters projectileSpawnParameters)
        {
            _soundProvider.PlayShootSound();
        }
    }

    
}
