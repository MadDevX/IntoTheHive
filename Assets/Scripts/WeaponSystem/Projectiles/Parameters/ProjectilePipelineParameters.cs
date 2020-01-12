using System.Collections.Generic;

public struct ProjectilePipelineParameters
{
    public IProjectile projectile;
    public List<IModule> modules;
    public List<IModule> inheritableModules;

    public ProjectilePipelineParameters(IProjectile projectile, List<IModule> modules, List<IModule> inheritableModules)
    {
        this.projectile = projectile;
        this.modules = modules;
        this.inheritableModules = inheritableModules;
    }

    public ProjectilePipelineParameters(IProjectile projectile, ProjectileSpawnParameters parameters)
    {
        this.projectile = projectile;
        this.modules = parameters.modules;
        this.inheritableModules = parameters.inheritableModules;
    }
}
