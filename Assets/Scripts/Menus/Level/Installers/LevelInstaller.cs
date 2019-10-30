using UnityEngine;
using Zenject;


public class LevelInstaller: MonoInstaller
{
    [SerializeField] private Rooms _rooms;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<BasicLevelGraphGenerator>();
        Container.BindInterfacesAndSelfTo<LevelSpawner>();
        Container.BindInstance(_rooms);
    }

}

