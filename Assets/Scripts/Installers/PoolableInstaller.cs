using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "PoolableInstaller", menuName = "Installers/PoolableInstaller")]
public class PoolableInstaller : ScriptableObjectInstaller<PoolableInstaller>
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private GameObject _rayProjectilePrefab;
    [SerializeField] private LineVFX _lineVFX;
    [SerializeField] private CharacterFacade _playerPrefab;
    [SerializeField] private CharacterFacade _networkedCharacterPrefab;
    [SerializeField] private CharacterFacade _AIPrefab;
    [SerializeField] private ItemPickup _pickupPrefab;
    [SerializeField] private ExplosionVFX _explosionVFX;

    public override void InstallBindings()
    {
        BindingCondition bindCond = (x) => x.Container == Container;

        Container.BindMonoContextPool<ProjectileFacade, ProjectileSpawnParameters, ProjectileFacade.Factory, ProjectilePool>
            (Identifiers.Bullet, 10, _projectilePrefab, "Projectiles", bindCond);

        Container.BindMonoContextPool<ProjectileFacade, ProjectileSpawnParameters, ProjectileFacade.Factory, ProjectilePool>
            (Identifiers.Ray, 10, _rayProjectilePrefab, "RayProjectiles", bindCond);

        Container.BindMonoPrefabPool<LineVFX, LineVFXSpawnParameters, LineVFX.Factory, LineVFXPool>
            (Identifiers.Ray, 10, _lineVFX, "LineVFXs");

        Container.BindMonoPrefabPool<ExplosionVFX, Vector3, ExplosionVFX.Factory, ExplosionVFXPool>
            (Identifiers.Explosion, 10, _explosionVFX, "ExplosionVFXs");

        Container.BindMonoContextPool<ItemPickup, PickupSpawnParameters, ItemPickup.Factory, PickupPool>
            (Identifiers.Inventory, 10, _pickupPrefab, "Pickups");

        Container.BindMonoPrefabPool<CharacterFacade, CharacterSpawnParameters, CharacterFacade.Factory, CharacterPool>
            (Identifiers.AI, 10, _AIPrefab, "AI");

        Container.BindMonoPrefabPool<CharacterFacade, CharacterSpawnParameters, CharacterFacade.Factory, CharacterPool>
            (Identifiers.Network, 4, _networkedCharacterPrefab, "Characters");

        Container.BindMonoPrefabPool<CharacterFacade, CharacterSpawnParameters, CharacterFacade.Factory, CharacterPool>
            (Identifiers.Player, 1, _playerPrefab, "Players");
        
        Container.Bind<IFactory<ProjectileSpawnParameters, ProjectileFacade[]>>().WithId(Identifiers.Bullet).To<RigidProjectileMultiFactory>().AsSingle();
        Container.Bind<IFactory<ProjectileSpawnParameters, ProjectileFacade[]>>().WithId(Identifiers.Ray).To<RayProjectileMultiFactory>().AsSingle();
    }

    public class ProjectilePool : ContextGameObjectMemoryPool<ProjectileSpawnParameters, IMemoryPool, ProjectileFacade>
    {
    }

    public class LineVFXPool : MonoPoolableMemoryPool<LineVFXSpawnParameters, IMemoryPool, LineVFX>
    {
    }

    public class ExplosionVFXPool : MonoPoolableMemoryPool<Vector3, IMemoryPool, ExplosionVFX>
    {
    }

    public class RigidProjectileMultiFactory : MultiFactory<ProjectileSpawnParameters, ProjectileFacade>
    {
        public RigidProjectileMultiFactory([Inject(Id = Identifiers.Bullet)] ProjectileFacade.Factory factory) : base(factory) { }
    }

    public class RayProjectileMultiFactory : MultiFactory<ProjectileSpawnParameters, ProjectileFacade>
    {
        public RayProjectileMultiFactory([Inject(Id = Identifiers.Ray)] ProjectileFacade.Factory factory) : base(factory) { }
    }

    public class CharacterPool : MonoPoolableMemoryPool<CharacterSpawnParameters, IMemoryPool, CharacterFacade>
    {
    }

    public class PickupPool : MonoPoolableMemoryPool<PickupSpawnParameters, IMemoryPool, ItemPickup>
    {
    }
}