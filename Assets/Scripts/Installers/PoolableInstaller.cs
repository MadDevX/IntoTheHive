using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "PoolableInstaller", menuName = "Installers/PoolableInstaller")]
public class PoolableInstaller : ScriptableObjectInstaller<PoolableInstaller>
{
    [SerializeField] private ProjectileFacade _projectilePrefab;
    [SerializeField] private RayProjectile _rayProjectilePrefab;

    public override void InstallBindings()
    {
        Container.BindFactory<ProjectileSpawnParameters, ProjectileFacade, ProjectileFacade.Factory>().
            WithId(Identifiers.Bullet).
            FromPoolableMemoryPool<ProjectileSpawnParameters, ProjectileFacade, ProjectilePool>
            (x => x.WithInitialSize(10).
            ExpandByDoubling().
            FromSubContainerResolve().
            ByNewContextPrefab(_projectilePrefab).
            UnderTransformGroup("Projectiles")).When((x) => x.Container == Container);

        Container.Bind<IFactory<ProjectileSpawnParameters, ProjectileFacade[]>>().WithId(Identifiers.Bullet).To<ProjectileFacade.MultiFactory>().AsSingle();
    }


    public class ProjectilePool : MonoPoolableMemoryPool<ProjectileSpawnParameters, IMemoryPool, ProjectileFacade>
    {
    }
}