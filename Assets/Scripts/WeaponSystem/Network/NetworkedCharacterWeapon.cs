using System;
using System.Collections.Generic;
using DarkRift;
using UnityEngine;
using Zenject;

public class NetworkedCharacterWeapon: IDisposable
{
    private NetworkRelay _networkRelay;
    private CharacterFacade _characterFacade;
    private IWeapon _weapon;
    private WeaponCreator _weaponCreator;

    public NetworkedCharacterWeapon(
        NetworkRelay networkRelay,
        CharacterFacade characterFacade,
        WeaponCreator weaponCreator,
        IWeapon weapon)
    {
        _weapon = weapon;
        _networkRelay = networkRelay;
        _characterFacade = characterFacade;
        _weaponCreator = weaponCreator;
    }

    [Inject]
    public void Initialize()
    {
        _networkRelay.Subscribe(Tags.WeaponChanged, HandleWeaponChanged);
    }
 
    public void Dispose()
    {
        _networkRelay.Unsubscribe(Tags.WeaponChanged, HandleWeaponChanged);
    }

    private void HandleWeaponChanged(Message message)
    {
        List<short> moduleIds = new List<short>();
        using (DarkRiftReader reader = message.GetReader())
        {
            //TODO MG CHECKSIZE
            ushort playerId = reader.ReadUInt16();            
            if(playerId == _characterFacade.Id)
            {                
                while (reader.Position < reader.Length)
                {
                    moduleIds.Add(reader.ReadInt16());
                }

                _weaponCreator.CreateWeapon(_weapon, moduleIds);
            }
        }
    }
}