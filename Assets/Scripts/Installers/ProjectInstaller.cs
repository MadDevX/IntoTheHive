using GameLoop.Internal;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private MonoGameLoop _gameLoop;
    [SerializeField] private Scenes _scenes;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    public override void InstallBindings()
    {
        Container.Bind<IGameLoop>().FromInstance(_gameLoop).AsSingle();
        Container.Bind<Scenes>().FromInstance(_scenes).AsSingle();
        Container.BindInterfacesAndSelfTo<ScenePostinitializationEvents>().AsSingle();
        Container.BindInterfacesAndSelfTo<ProjectEventManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerRegistry>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameCycle>().AsSingle();
        Container.BindInterfacesAndSelfTo<WinManager>().AsSingle();           
        Container.BindInterfacesAndSelfTo<GameState>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameplayStateManager>().AsSingle();
    }
}
