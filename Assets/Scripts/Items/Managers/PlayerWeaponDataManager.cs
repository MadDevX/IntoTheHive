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
    private ClientInfo _info;
    private IGameCycle _cycle;

    private Dictionary<ushort, List<short>> _weapons = new Dictionary<ushort, List<short>>();

    public PlayerWeaponDataManager(NetworkRelay networkRelay, ItemDatabase database, ClientInfo info, IGameCycle cycle)
    {
        _networkRelay = networkRelay;
        _database = database;
        _info = info;
        _cycle = cycle;
    }

    public void Initialize()
    {
        _networkRelay.Subscribe(Tags.WeaponChanged, UpdateWeapon);
        _cycle.OnGameEnded += ResetData;
    }

    public void Dispose()
    {
        _networkRelay.Unsubscribe(Tags.WeaponChanged, UpdateWeapon);
        _cycle.OnGameEnded -= ResetData;
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
        if (_info.Status == ClientStatus.Host)
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

    private void ResetData()
    {
        _weapons.Clear();
    }

}
