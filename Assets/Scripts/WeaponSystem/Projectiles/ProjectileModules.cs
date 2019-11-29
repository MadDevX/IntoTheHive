using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileModules
{
    private IProjectile _facade;

    private List<IModule> _currentModules = new List<IModule>();

    public ProjectileModules(IProjectile facade)
    {
        _facade = facade;
    }


    public void SetModules(List<IModule> modules)
    {
        _currentModules.Clear();
        _currentModules.AddRange(modules);
        InitModules();
    }


    private void InitModules()
    {
        for (int i = 0; i < _currentModules.Count; i++)
        {
            _currentModules[i].DecorateProjectile(_facade);
        }
    }

    private void DisposeModules()
    {
        for (int i = _currentModules.Count - 1; i >= 0; i--)
        {
            _currentModules[i].RemoveFromProjectile(_facade);
        }
    }
}
