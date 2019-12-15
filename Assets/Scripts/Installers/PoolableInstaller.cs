using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "PoolableInstaller", menuName = "Installers/PoolableInstaller")]
public class PoolableInstaller : ScriptableObjectInstaller<PoolableInstaller>
{
    [SerializeField] private ProjectileFacade _projectilePrefab;
    [SerializeField] private ProjectileFacade _rayProjectilePrefab;
    [SerializeField] private LineVFX _lineVFX;
    [SerializeField] private CharacterFacade _playerPrefab;
    [SerializeField] private CharacterFacade _networkedCharacterPrefab;
    [SerializeField] private CharacterFacade _AIPrefab;

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
        BindingCondition bindCond = (x) => x.Container == Container;
        BindMonoContextPool<ProjectileFacade, ProjectileSpawnParameters, ProjectileFacade.Factory, ProjectilePool>
            (Identifiers.Bullet, 10, _projectilePrefab, "Projectiles", bindCond);

        BindMonoContextPool<ProjectileFacade, ProjectileSpawnParameters, ProjectileFacade.Factory, ProjectilePool>
            (Identifiers.Ray, 10, _rayProjectilePrefab, "RayProjectiles", bindCond);

        BindMonoPrefabPool<LineVFX, LineVFXSpawnParameters, LineVFX.Factory, LineVFXPool>
            (Identifiers.Ray, 10, _lineVFX, "LineVFXs");

        BindMonoPrefabPool<CharacterFacade, CharacterSpawnParameters, CharacterFacade.Factory, CharacterPool>
            (Identifiers.AI, 10, _AIPrefab, "AI");

        BindMonoPrefabPool<CharacterFacade, CharacterSpawnParameters, CharacterFacade.Factory, CharacterPool>
            (Identifiers.Network, 4, _networkedCharacterPrefab, "Characters");

        BindMonoPrefabPool<CharacterFacade, CharacterSpawnParameters, CharacterFacade.Factory, CharacterPool>
            (Identifiers.Player, 1, _playerPrefab, "Players");
        
        Container.Bind<IFactory<ProjectileSpawnParameters, ProjectileFacade[]>>().WithId(Identifiers.Bullet).To<RigidProjectileMultiFactory>().AsSingle();
        Container.Bind<IFactory<ProjectileSpawnParameters, ProjectileFacade[]>>().WithId(Identifiers.Ray).To<RayProjectileMultiFactory>().AsSingle();
    }

    private void BindMonoContextPool<T, TArgs, TFactory, TPool>(Identifiers id, int size, T prefab, string transformGroupName, BindingCondition cond = null) 
        where T : MonoBehaviour, IPoolable<TArgs, IMemoryPool>
        where TFactory : PlaceholderFactory<TArgs, T>
        where TPool : MonoPoolableMemoryPool<TArgs, IMemoryPool, T>
    {
        var bind =
        Container.BindFactory<TArgs, T, TFactory>().
            WithId(id).
            FromPoolableMemoryPool<TArgs, T, TPool>
            (x => x.WithInitialSize(size).
            ExpandByDoubling().
            FromSubContainerResolve().
            ByNewContextPrefab(prefab).
            UnderTransformGroup(transformGroupName));

        if (cond != null)
        {
            bind.When(cond);
        }
    }

    private void BindMonoPrefabPool<T, TArgs, TFactory, TPool>(Identifiers id, int size, T prefab, string transformGroupName, BindingCondition cond = null)
    where T : MonoBehaviour, IPoolable<TArgs, IMemoryPool>
    where TFactory : PlaceholderFactory<TArgs, T>
    where TPool : MonoPoolableMemoryPool<TArgs, IMemoryPool, T>
    {
        var bind = 
        Container.BindFactory<TArgs, T, TFactory>().
            WithId(id).
            FromPoolableMemoryPool<TArgs, T, TPool>
            (x => x.WithInitialSize(size).
            ExpandByDoubling().
            FromComponentInNewPrefab(prefab).
            UnderTransformGroup(transformGroupName));

        if (cond != null)
        {
            bind.When(cond);
        }
    }

    public class ProjectilePool : MonoPoolableMemoryPool<ProjectileSpawnParameters, IMemoryPool, ProjectileFacade>
    {
    }

    public class LineVFXPool : MonoPoolableMemoryPool<LineVFXSpawnParameters, IMemoryPool, LineVFX>
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

    public class CharacterPool : MonoPoolableMemoryPool<CharacterSpawnParameters, IMemoryPool, CharacterFacade>
    {
    }
}