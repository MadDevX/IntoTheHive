using GameLoop;
using System;
using UnityEngine;
using Zenject;

public class DoorFacade: MonoBehaviour
{
    public void Despawn()
    {
        gameObject.SetActive(false);
    }

    public class Factory: PlaceholderFactory<DoorSpawnParameters, DoorFacade>
    {
    }
}