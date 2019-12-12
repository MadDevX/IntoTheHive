using System;

public interface IProjectileHit
{
    event Action<IHealth> OnHit;
}