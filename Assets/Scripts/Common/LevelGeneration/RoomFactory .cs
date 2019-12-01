﻿using System;
using UnityEngine;
using Zenject;

/// <summary>
/// This class spawns rooms based on spawn parameters
/// </summary>
public class RoomFactory : IFactory<RoomSpawnParameters, RoomFacade>
{
    private DiContainer _container;
    private Rooms _rooms;

    public RoomFactory(
        DiContainer container,
        Rooms rooms)
    {
        _rooms = rooms;
        _container = container;
    }

    public RoomFacade Create(RoomSpawnParameters param)
    {

        var roomPrefab = _rooms.GetRoomById(param.RoomId);
        var instantiatedObject = _container.InstantiatePrefab(roomPrefab);

        Transform transform = instantiatedObject.transform;
        transform.position.Set(param.X, param.X, transform.position.z);

        if(param.IsHorizontal)
        {
            transform.Rotate(0, 0, 90);   
        }

        return instantiatedObject.GetComponent<RoomFacade>();
    }
}