using UnityEngine;
using Zenject;

public class DoorFactory: IFactory<DoorSpawnParameters, DoorFacade>
{
    private Doors _doors;
    private DiContainer _container;

    public DoorFactory(
        DiContainer container,
        Doors doors)
    {
        _container = container;
        _doors = doors;
    }

    public DoorFacade Create(DoorSpawnParameters param)
    {
        var doorPrefab = _doors.BasicDoor;
        var instantiatedObject = _container.InstantiatePrefab(doorPrefab);

        Transform transform = instantiatedObject.transform;
        transform.SetPositionAndRotation(new Vector3(param.X, param.Y, transform.position.z), transform.rotation);

        if (param.IsHorizontal)
        {
            transform.Rotate(0, 0, 90);
        }

        return instantiatedObject.GetComponent<DoorFacade>();
    }
}