using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "PoolableInstaller", menuName = "Installers/PoolableInstaller")]
public class PoolableInstaller : ScriptableObjectInstaller<PoolableInstaller>
{

    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private CharacterFacade _playerPrefab;
    [SerializeField] private CharacterFacade _networkedCharacterPrefab;
    [SerializeField] private CharacterFacade _AIPrefab;

    public override void InstallBindings()
    {

        // TODO generic methodd to bind factories with generic parameters
        Container.BindFactory<ProjectileSpawnParameters, Projectile, Projectile.Factory>().
            FromPoolableMemoryPool<ProjectileSpawnParameters, Projectile, ProjectilePool>
            (x => x.WithInitialSize(10).
            ExpandByDoubling().
            FromComponentInNewPrefab(_projectilePrefab).
            UnderTransformGroup("Projectiles"));

        //TODO check if factory binding is correct ("FromSubContainerResolve") and the following methods "Bynew contextPrefabs"
        Container.BindFactory<CharacterSpawnParameters, CharacterFacade, CharacterFacade.Factory>().
           WithId(Identifiers.AI).
           FromPoolableMemoryPool<CharacterSpawnParameters, CharacterFacade, CharacterPool>
           (x => x.WithInitialSize(4).
           ExpandByDoubling().
           FromSubContainerResolve().
           ByNewContextPrefab(_AIPrefab).
           UnderTransformGroup("AI"));

        Container.BindFactory<CharacterSpawnParameters, CharacterFacade, CharacterFacade.Factory>().
          WithId(Identifiers.Network).
          FromPoolableMemoryPool<CharacterSpawnParameters, CharacterFacade, CharacterPool>
          (x => x.WithInitialSize(4).
          ExpandByDoubling().
          FromSubContainerResolve().
          ByNewContextPrefab(_networkedCharacterPrefab).
          UnderTransformGroup("Characters"));

        Container.BindFactory<CharacterSpawnParameters, CharacterFacade, CharacterFacade.Factory>().
          WithId(Identifiers.Player).
          FromPoolableMemoryPool<CharacterSpawnParameters, CharacterFacade, CharacterPool>
          (x => x.WithInitialSize(1).
          FromSubContainerResolve().
          ByNewContextPrefab(_playerPrefab).
          UnderTransformGroup("Player")); 

    }


    public class ProjectilePool : MonoPoolableMemoryPool<ProjectileSpawnParameters, IMemoryPool, Projectile>
    {
    }

    public class CharacterPool : MonoPoolableMemoryPool<CharacterSpawnParameters, IMemoryPool, CharacterFacade>
    {
    }
}