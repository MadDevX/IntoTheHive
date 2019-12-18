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

    private List<IModule> CreateModuleList(List<short> modulesIds)
    {
        _holdingList.Clear();
        for(int i = 0; i < modulesIds.Count; i++)
        {
            _holdingList.Add(_factory.Create(modulesIds[i]));
        }
        return _holdingList;
    }
}
