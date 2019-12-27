using UnityEngine;
using Zenject;


public class LevelInstaller: MonoInstaller
{
    [SerializeField] private AstarPath _astarModule;
    public override void InstallBindings()
    {
        //Container.BindInterfacesAndSelfTo<HostEncounterManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameEndManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<AIGraphSpawner>().AsSingle();
        Container.BindInstance(_astarModule);
    }

}

