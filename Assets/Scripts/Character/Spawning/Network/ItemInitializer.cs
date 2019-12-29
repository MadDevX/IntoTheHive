using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

/// <summary>
/// Prepares spawn data to contain items and active modules for each player during a single run, used for respawning across different scenes.
/// </summary>
public class ItemInitializer : IInitializable, IDisposable
{
    private NetworkedCharacterSpawner _spawner;
    private PlayerWeaponDataManager _weaponData;
    private PlayerInventoryDataManager _invData;

    public ItemInitializer(NetworkedCharacterSpawner spawner, PlayerWeaponDataManager weaponData, PlayerInventoryDataManager invData)
    {
        _spawner = spawner;
        _weaponData = weaponData;
        _invData = invData;
    }

    public void Initialize()
    {
        _spawner.OnDataPrepared += OnDataPrepared;
    }

    public void Dispose()
    {
        _spawner.OnDataPrepared -= OnDataPrepared;
    }

    private void OnDataPrepared(List<PlayerSpawnData> dataList)
    {
        for(int i = 0; i < dataList.Count; i++)
        {
            InitializeInventoryData(dataList[i]);
            InitializeWeaponData(dataList[i]);
        }
    }

    private void InitializeInventoryData(PlayerSpawnData data)
    {
        var items = _invData.GetData(data.Id);
        if(items != null)
        {
            for(int i = 0; i < items.Count; i++)
            {
                data.ItemIds.Add(items[i].itemId);
            }
        }
    }

    private void InitializeWeaponData(PlayerSpawnData data)
    {
        var modules = _weaponData.GetData(data.Id);
        if(modules != null)
        {
            data.WeaponModuleIds.AddRange(modules);
        }
    }
}
