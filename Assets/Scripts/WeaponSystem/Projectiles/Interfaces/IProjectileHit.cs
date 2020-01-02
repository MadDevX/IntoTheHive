using System;

public interface IProjectileHit
{
    event Action<HitParameters> OnHit;
}