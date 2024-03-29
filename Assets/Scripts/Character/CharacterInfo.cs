﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : IDisposable
{
    public CharacterType Type { get; private set; }
    public bool IsLocal { get; private set; }
    public ushort Id { get; private set; }

    private IRespawnable<CharacterSpawnParameters> _respawnable;

    public CharacterInfo(IRespawnable<CharacterSpawnParameters> respawnable)
    {
        _respawnable = respawnable;
        PreInitialize();
    }

    private void PreInitialize()
    {
        _respawnable.OnSpawn += OnSpawn;
    }
    public void Dispose()
    {
        _respawnable.OnSpawn -= OnSpawn;
    }

    private void OnSpawn(CharacterSpawnParameters obj)
    {
        Type = obj.CharacterType;
        IsLocal = obj.IsLocal;
        Id = obj.Id;
    }

}
