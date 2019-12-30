using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedCharacterWeaponInitalizer : IDisposable
{
    private IRespawnable<CharacterSpawnParameters> _respawnable;
    private WeaponCreator _creator;
    private IWeapon _weapon;

    public NetworkedCharacterWeaponInitalizer(IRespawnable<CharacterSpawnParameters> respawnable, WeaponCreator creator, IWeapon weapon)
    {
        _respawnable = respawnable;
        _creator = creator;
        _weapon = weapon;
        PreInitialize();
    }

    private void PreInitialize()
    {
        _respawnable.OnSpawn += InitializeInventory;
    }

    public void Dispose()
    {
        _respawnable.OnSpawn -= InitializeInventory;
    }

    private void InitializeInventory(CharacterSpawnParameters obj)
    {
        _creator.CreateWeapon(_weapon, obj.modules);
    }
}
