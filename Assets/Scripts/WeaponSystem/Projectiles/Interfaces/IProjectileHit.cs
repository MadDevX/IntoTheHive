using System;

public interface IProjectileHit
{
    event Action<IDamageable> OnHit;
}