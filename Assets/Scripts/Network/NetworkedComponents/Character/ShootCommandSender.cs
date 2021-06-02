using DarkRift;
using DarkRift.Client.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootCommandSender : IDisposable
{
    private UnityClient _client;
    private IWeapon _weapon;
    private CharacterInfo _info;

    public ShootCommandSender(UnityClient client, IWeapon weapon, CharacterInfo info)
    {
        _client = client;
        _weapon = weapon;
        _info = info;
        PreInitialize();
    }

    private void PreInitialize()
    {
        _weapon.OnShoot += OnShoot;
    }
    public void Dispose()
    {
        _weapon.OnShoot -= OnShoot;
    }

    private void OnShoot(ProjectileSpawnParameters obj)
    {
        if (_info.IsLocal)
        {
            using (var writer = DarkRiftWriter.Create())
            {
                writer.Write(_info.Id);
                writer.Write(obj.position.x);
                writer.Write(obj.position.y);
                writer.Write(obj.rotation);

                using (var message = Message.Create(Tags.SpawnProjectile, writer))
                {
                    _client.SendMessage(message, SendMode.Reliable);
                }
            }
        }
    }

}
