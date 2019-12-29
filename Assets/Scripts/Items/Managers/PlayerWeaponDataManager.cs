using System;
using System.Collections;
using System.Collections.Generic;
using DarkRift;
using UnityEngine;
using Zenject;

public class PlayerWeaponDataManager : IInitializable, IDisposable
{
    private NetworkRelay _networkRelay;
    private ItemDatabase _database;

    private Dictionary<ushort, List<short>> _weapons;

    public PlayerWeaponDataManager(NetworkRelay networkRelay, ItemDatabase database)
    {
        _networkRelay = networkRelay;
        _database = database;
    }

    public void Initialize()
    {
        _networkRelay.Subscribe(Tags.WeaponChanged, UpdateWeapon);
    }

    public void Dispose()
    {
        _networkRelay.Unsubscribe(Tags.WeaponChanged, UpdateWeapon);
    }

    public List<short> GetData(ushort id)
    {
        if(_weapons.TryGetValue(id, out var list))
        {
            return list;
        }
        else
        {
            return null;
        }
    }

    private void UpdateWeapon(Message message)
    {
        using (DarkRiftReader reader = message.GetReader())
        {
            ushort playerId = reader.ReadUInt16();

            if (_weapons.TryGetValue(playerId, out var moduleIds) == false)
            {
                moduleIds = new List<short>();
                _weapons.Add(playerId, moduleIds);
            }
            else
            {
                moduleIds.Clear(); //if entry exists, weaponchanged contains all current modules - thus list clearing
            }
            while (reader.Position < reader.Length)
            {
                moduleIds.Add(reader.ReadInt16());
            }
        }
    }

}
