using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "PoolableInstaller", menuName = "Installers/PoolableInstaller")]
public class PoolableInstaller : ScriptableObjectInstaller<PoolableInstaller>
{
    [SerializeField] private ProjectileFacade _projectilePrefab;
    [SerializeField] private ProjectileFacade _rayProjectilePrefab;

    public override void InstallBindings()
    {
        //Container.BindFactory<ProjectileSpawnParameters, ProjectileFacade, ProjectileFacade.Factory>().
        //    WithId(Identifiers.Bullet).
        //    FromPoolableMemoryPool<ProjectileSpawnParameters, ProjectileFacade, ProjectilePool>
        //    (x => x.WithInitialSize(10).
        //    ExpandByDoubling().
        //    FromSubContainerResolve().
        //    ByNewContextPrefab(_projectilePrefab).
        //    UnderTransformGroup("Projectiles")).When((x) => x.Container == Container);
        BindMonoPool<ProjectileFacade, ProjectileSpawnParameters, ProjectileFacade.Factory, ProjectilePool>
            (Identifiers.Bullet, 10, _projectilePrefab, "Projectiles");

        BindMonoPool<ProjectileFacade, ProjectileSpawnParameters, ProjectileFacade.Factory, ProjectilePool>
            (Identifiers.Ray, 10, _rayProjectilePrefab, "RayProjectiles");

        Container.Bind<IFactory<ProjectileSpawnParameters, ProjectileFacade[]>>().WithId(Identifiers.Bullet).To<RigidProjectileMultiFactory>().AsSingle();
        Container.Bind<IFactory<ProjectileSpawnParameters, ProjectileFacade[]>>().WithId(Identifiers.Ray).To<RayProjectileMultiFactory>().AsSingle();
    }



    private void BindMonoPool<T, TArgs, TFactory, TPool>(Identifiers id, int size, T prefab, string transformGroupName) 
        where T : MonoBehaviour, IPoolable<TArgs, IMemoryPool>
        where TFactory : PlaceholderFactory<TArgs, T>
        where TPool : MonoPoolableMemoryPool<TArgs, IMemoryPool, T>
    {
        Container.BindFactory<TArgs, T, TFactory>().
            WithId(id).
            FromPoolableMemoryPool<TArgs, T, TPool>
            (x => x.WithInitialSize(size).
            ExpandByDoubling().
            FromSubContainerResolve().
            ByNewContextPrefab(prefab).
            UnderTransformGroup(transformGroupName)).When((x) => x.Container == Container);
    }

    public class ProjectilePool : MonoPoolableMemoryPool<ProjectileSpawnParameters, IMemoryPool, ProjectileFacade>
    {
    }

    public class RigidProjectileMultiFactory : MultiFactory<ProjectileSpawnParameters, ProjectileFacade> //TODO: possibly move into separate file and define multiple factories for every projectile prefab (rigid, ray, etc)
    {
        public RigidProjectileMultiFactory([Inject(Id = Identifiers.Bullet)] ProjectileFacade.Factory factory) : base(factory) { }
    }

    public class RayProjectileMultiFactory : MultiFactory<ProjectileSpawnParameters, ProjectileFacade> //TODO: possibly move into separate file and define multiple factories for every projectile prefab (rigid, ray, etc)
    {
        public RayProjectileMultiFactory([Inject(Id = Identifiers.Ray)] ProjectileFacade.Factory factory) : base(factory) { }
    }
}