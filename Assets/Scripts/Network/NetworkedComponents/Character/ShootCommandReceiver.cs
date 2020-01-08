using System;
using System.Collections;
using System.Collections.Generic;
using DarkRift;
using UnityEngine;

public class ShootCommandReceiver : IDisposable
{
    private NetworkRelay _networkRelay;
    private CharacterInfo _info;
    private IWeapon _weapon;
    public ShootCommandReceiver(NetworkRelay networkRelay, CharacterInfo info, IWeapon weapon)
    {
        _networkRelay = networkRelay;
        _info = info;
        _weapon = weapon;
        PreInitialize();
    }

    private void PreInitialize()
    {
        _networkRelay.Subscribe(Tags.SpawnProjectile, HandleMessage);
    }

    public void Dispose()
    {
        _networkRelay.Unsubscribe(Tags.SpawnProjectile, HandleMessage);
    }

    private void HandleMessage(Message obj)
    {
        using(var reader = obj.GetReader())
        {
            var characterId = reader.ReadUInt16();
            if(characterId == _info.Id && _info.IsLocal == false)
            {
                var posX = reader.ReadSingle();
                var posY = reader.ReadSingle();
                var rot = reader.ReadSingle();
                _weapon.Shoot(new Vector2(posX, posY), rot);
            }
        }
    }
}
