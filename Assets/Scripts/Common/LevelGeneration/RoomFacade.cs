using GameLoop;
using UnityEngine;
using Zenject;

/// <summary>
/// This class is used to reference rooms in code
/// </summary>
public class RoomFacade: MonoUpdatableObject
{
    public override void OnUpdate(float deltaTime)
    {
    }

    public class Factory: PlaceholderFactory<RoomSpawnParameters, RoomFacade>
    {
    }
}

