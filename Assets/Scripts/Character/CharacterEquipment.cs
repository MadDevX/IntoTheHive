using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEquipment
{
    public IWeapon Weapon { get; set; }

    public CharacterEquipment(IWeapon weapon)
    {
        Weapon = weapon;
    }
}
