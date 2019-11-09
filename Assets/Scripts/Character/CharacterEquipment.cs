using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEquipment
{
    public IWeapon Weapon { get; set; }

    // placeholder for event which fires when the weapon is changed
    // insert IWeapon as an argument?
    public event Action<IWeapon> OnWeaponChanged;

    public CharacterEquipment(IWeapon weapon)
    {
        Weapon = weapon;
    }
}
