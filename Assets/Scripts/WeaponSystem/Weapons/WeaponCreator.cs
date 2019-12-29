using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCreator
{
    private ModuleFactory _factory;

    private List<IModule> _holdingList = new List<IModule>();
    public WeaponCreator(ModuleFactory factory)
    {
        _factory = factory;
    }

    public void CreateWeapon(IWeapon weapon, List<short> modulesIds)
    {
        weapon.SetModules(CreateModuleList(modulesIds));
    }
    public void CreateWeapon(IWeapon weapon, List<IModule> modulesIds)
    {
        weapon.SetModules(modulesIds);
    }

    private List<IModule> CreateModuleList(List<short> modules)
    {
        _holdingList.Clear();
        for(int i = 0; i < modules.Count; i++)
        {
            _holdingList.Add(_factory.Create(modules[i]));
        }
        return _holdingList;
    }
}
