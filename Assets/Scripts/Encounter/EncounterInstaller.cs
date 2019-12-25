using UnityEngine;
using Zenject;

public class EncounterInstaller: MonoInstaller
{
    [SerializeField] private RoomEnemyInfoPool _infoPool;

    public override void InstallBindings()
    {
        // Scene context
        Container.BindInterfacesAndSelfTo<HostEncounterManager>().AsSingle();
        // Scene context
        Container.BindInterfacesAndSelfTo<HostEncounterEnemyManager>().AsSingle();
        // GameObject context
        Container.BindInstances(_infoPool);
        //Container.BindInterfacesAndSelfTo<RoomEnemyInfoPool>().AsSingle();
    }
}