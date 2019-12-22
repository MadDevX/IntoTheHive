using GameLoop;
using UnityEngine;
using Zenject;

/// <summary>
/// This class is used to reference rooms in code
/// </summary>
public class RoomFacade: MonoBehaviour
{
    public ushort ID { get; set; }
    public bool Visited { get; set; }

    public class Factory: PlaceholderFactory<RoomSpawnParameters, RoomFacade>
    {
    }
}

