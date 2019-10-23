using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "PoolableInstaller", menuName = "Installers/PoolableInstaller")]
public class PoolableInstaller : ScriptableObjectInstaller<PoolableInstaller>
{
    [SerializeField] private Projectile _projectilePrefab;

    public override void InstallBindings()
    {
        Container.BindFactory<ProjectileSpawnParameters, Projectile, Projectile.Factory>().
            FromPoolableMemoryPool<ProjectileSpawnParameters, Projectile, ProjectilePool>
            (x => x.WithInitialSize(10).
            ExpandByDoubling().
            FromComponentInNewPrefab(_projectilePrefab).
            UnderTransformGroup("Projectiles"));
    }


    public class ProjectilePool : MonoPoolableMemoryPool<ProjectileSpawnParameters, IMemoryPool, Projectile>
    {
    }
}