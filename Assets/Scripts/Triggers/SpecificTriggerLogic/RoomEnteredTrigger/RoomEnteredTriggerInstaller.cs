using UnityEngine;
using Zenject;

public class RoomEnteredTriggerInstaller: MonoInstaller
{
    [SerializeField] public RoomFacade _roomFacade;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<RoomEnteredTrigger>().AsSingle();
        Container.BindInstance(_roomFacade);
    }
}