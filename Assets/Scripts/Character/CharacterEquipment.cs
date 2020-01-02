using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Stub to be used in the future - current state of character only has a Weapon - thus forwarding from Equipment is not necessary
/// </summary>
public class CharacterEquipment
{
    public IWeapon Weapon { get; set; }

    public CharacterEquipment(IWeapon weapon)
    {
        Weapon = weapon;
    }
}
