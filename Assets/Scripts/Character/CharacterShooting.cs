using GameLoop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShooting : UpdatableObject
{
    private ControlState _controlState;
    private IWeapon _weapon;
    private Rigidbody2D _rb;
    private Settings _settings;
    public CharacterShooting(ControlState controlState, IWeapon weapon, Rigidbody2D rb, Settings settings)
    {
        _controlState = controlState;
        _weapon = weapon;
        _rb = rb;
        _settings = settings;
    }

    public override void OnUpdate(float deltaTime)
    {
        if(_controlState.PrimaryAction)
        {
            if (_weapon.Shoot(_rb.position, _rb.rotation, _settings.weaponOffset) == false)
            {
                _weapon.Reload();
            }
        }
        else
        {
            _weapon.ReleaseTrigger();
        }
    }

    [System.Serializable]
    public class Settings
    {
        public Vector2 weaponOffset;
    }
}
