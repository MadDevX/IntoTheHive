using System.Collections.Generic;
using UnityEngine;

public struct ProjectileSpawnParameters
{
    public Vector2 position;
    public float rotation;
    public float velocity;
    public float ttl;
    public List<IModule> modules;
    public List<IModule> inheritableModules;
    public bool dummy;

    public ProjectileSpawnParameters(
        Vector2 position, 
        float rotation, 
        float velocity, 
        float ttl, 
        List<IModule> modules, 
        List<IModule> inheritableModules, 
        bool dummy)
    {
        this.position = position;
        this.rotation = rotation;
        this.velocity = velocity;
        this.ttl = ttl;
        this.modules = modules;
        this.inheritableModules = inheritableModules;
        this.dummy = dummy;
    }
}
