using GameLoop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShooting : UpdatableObject
{
    private ControlState _controlState;
    private CharacterEquipment _characterEquipment;
    private Rigidbody2D _rb;
    private Settings _settings;
    public CharacterShooting(ControlState controlState, CharacterEquipment characterEquipment, Rigidbody2D rb, Settings settings)
    {
        _controlState = controlState;
        _characterEquipment = characterEquipment;
        _rb = rb;
        _settings = settings;
    }

    public override void OnUpdate(float deltaTime)
    {
        if(_controlState.PrimaryAction)
        {
            if (_characterEquipment.Weapon.Shoot(_rb.position, _rb.rotation, _settings.weaponOffset) == false)
            {
                _characterEquipment.Weapon.Reload();
            }
        }
        else
        {
            _characterEquipment.Weapon.ReleaseTrigger();
        }
    }

    [System.Serializable]
    public class Settings
    {
        public Vector2 weaponOffset;
    }
}
