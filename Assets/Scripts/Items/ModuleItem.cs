using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleItem : IItem
{
    public IModule Module { get; private set; }
    public bool IsEquipped => Module.IsAttached;

    public ModuleItem(IModule module)
    {
        Module = module;
    }

    public void UseItem(CharacterFacade facade)
    {
        if (Module.IsAttached == false)
        {
            facade.Weapon.AttachModule(Module);
        }
        else
        {
            facade.Weapon.DetachModule(Module);
        }
    }
}
