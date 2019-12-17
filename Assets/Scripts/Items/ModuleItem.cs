using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleItem : IItem
{
    private IModule _module;
    public bool IsActive => _module.IsAttached;

    public ModuleItem(IModule module)
    {
        _module = module;
    }

    public void UseItem(CharacterFacade facade)
    {
        if (_module.IsAttached == false)
        {
            facade.Weapon.AttachModule(_module);
        }
        else
        {
            facade.Weapon.DetachModule(_module);
        }
    }
}
