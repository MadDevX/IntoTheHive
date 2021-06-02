using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRegistry
{
    public event Action<CharacterFacade> OnPlayerSet;
    public event Action<CharacterFacade> OnPlayerUnset;

    private CharacterFacade _player;

    public CharacterFacade Player
    {
        get
        {
            return _player;
        }
        set
        {
            if(_player != null) OnPlayerUnset?.Invoke(_player);
            _player = value;
            if(_player != null) OnPlayerSet?.Invoke(_player);
        }
    }
}
