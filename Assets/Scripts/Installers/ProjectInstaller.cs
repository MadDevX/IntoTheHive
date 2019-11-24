using GameLoop.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private MonoGameLoop _gameLoop;
    [SerializeField] private Scenes _scenes;

    public override void InstallBindings()
    {
        Container.Bind<IGameLoop>().FromInstance(_gameLoop).AsSingle();
        Container.Bind<Scenes>().FromInstance(_scenes).AsSingle();
        Container.BindInterfacesAndSelfTo<ScenePostinitializationEvents>().AsSingle();
    }
}
