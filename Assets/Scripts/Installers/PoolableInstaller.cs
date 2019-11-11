using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "PoolableInstaller", menuName = "Installers/PoolableInstaller")]
public class PoolableInstaller : ScriptableObjectInstaller<PoolableInstaller>
{
    [SerializeField] private Projectile _projectilePrefab;

    public override void InstallBindings()
    {
        Container.BindFactory<ProjectileSpawnParameters, Projectile, Projectile.Factory>().
            WithId(Identifiers.Bullet).
            FromPoolableMemoryPool<ProjectileSpawnParameters, Projectile, ProjectilePool>
            (x => x.WithInitialSize(10).
            ExpandByDoubling().
            FromComponentInNewPrefab(_projectilePrefab).
            UnderTransformGroup("Projectiles")).When((x) => x.Container == Container);

        Container.Bind<IFactory<ProjectileSpawnParameters, Projectile[]>>().WithId(Identifiers.Bullet).To<Projectile.MultiFactory>().AsSingle();
    }


    public class ProjectilePool : MonoPoolableMemoryPool<ProjectileSpawnParameters, IMemoryPool, Projectile>
    {
    }
}