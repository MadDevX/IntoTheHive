using UnityEngine;
using Zenject;

/// <summary>
/// This class is used to reference rooms in code
/// </summary>
public class RoomFacade
{

    public class Factory: PlaceholderFactory<RoomSpawnParameters, RoomFacade>
    {
    }
}

