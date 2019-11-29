using System.Collections.Generic;

public static class ProjectileUtility
{
    public static void InitModules(List<IModule> modules, IProjectile projectile)
    {
        for (int i = 0; i < modules.Count; i++)
        {
            modules[i].DecorateProjectile(projectile);
        }
    }

    public static void DisposeModules(List<IModule> modules, IProjectile projectile)
    {
        for (int i = modules.Count - 1; i >= 0; i--)
        {
            modules[i].RemoveFromProjectile(projectile);
        }
    }
}
