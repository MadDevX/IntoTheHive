using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TripleShot : IModule
{
    public int Priority => 0;

    public void DecorateProjectile(Projectile projectile)
    {
        throw new System.NotImplementedException();
    }

    public void Initialize(Weapon weapon)
    {
        throw new System.NotImplementedException();
    }

    public void Initialize()
    {
        throw new System.NotImplementedException();
    }

    public class Factory : IFactory<ProjectileSpawnParameters, Projectile>
    {
        private IFactory<ProjectileSpawnParameters, Projectile> _decoratedFactory;
        private float _spreadAngle = 30.0f;
        public Factory(IFactory<ProjectileSpawnParameters, Projectile> decoratedFactory)
        {
            _decoratedFactory = decoratedFactory;
        }

        //check this shit out
        public /*List<*/Projectile/*>*/ Create(ProjectileSpawnParameters param)
        {
            var initRotation = param.rotation;
            var mainProjectile = _decoratedFactory.Create(param);
            param.rotation = initRotation + _spreadAngle;
            _decoratedFactory.Create(param);
            param.rotation = initRotation - _spreadAngle;
            _decoratedFactory.Create(param);
            return mainProjectile;
        }
    }
}
