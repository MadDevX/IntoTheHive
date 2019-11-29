using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "PoolableInstaller", menuName = "Installers/PoolableInstaller")]
public class PoolableInstaller : ScriptableObjectInstaller<PoolableInstaller>
{
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private RayProjectile _rayProjectilePrefab;

    public override void InstallBindings()
    {
        Container.BindFactory<ProjectileSpawnParameters, Projectile, Projectile.Factory>().
            WithId(Identifiers.Bullet).
            FromPoolableMemoryPool<ProjectileSpawnParameters, Projectile, ProjectilePool>
            (x => x.WithInitialSize(10).
            ExpandByDoubling().
            FromComponentInNewPrefab(_projectilePrefab).
            UnderTransformGroup("Projectiles")).When((x) => x.Container == Container);

        Container.BindFactory<ProjectileSpawnParameters, RayProjectile, RayProjectile.Factory>().
             WithId(Identifiers.Bullet).
             FromPoolableMemoryPool<ProjectileSpawnParameters, RayProjectile, RayProjectilePool>
             (x => x.WithInitialSize(10).
             ExpandByDoubling().
             FromComponentInNewPrefab(_rayProjectilePrefab).
             UnderTransformGroup("RayProjectiles")).When((x) => x.Container == Container);

        Container.Bind<IFactory<ProjectileSpawnParameters, Projectile[]>>().WithId(Identifiers.Bullet).To<Projectile.MultiFactory>().AsSingle();
        Container.Bind<IFactory<ProjectileSpawnParameters, RayProjectile[]>>().WithId(Identifiers.Bullet).To<RayProjectile.MultiFactory>().AsSingle();
    }


    public class ProjectilePool : MonoPoolableMemoryPool<ProjectileSpawnParameters, IMemoryPool, Projectile>
    {
    }

    public class RayProjectilePool : MonoPoolableMemoryPool<ProjectileSpawnParameters, IMemoryPool, RayProjectile>
    {
    }
}