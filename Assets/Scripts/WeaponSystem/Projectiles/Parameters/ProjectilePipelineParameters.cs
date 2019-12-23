public struct ProjectilePipelineParameters
{
    public IProjectile projectile;

    //TODO: collision information (CastHit)
    public ProjectilePipelineParameters(IProjectile projectile)
    {
        this.projectile = projectile;
    }
}
