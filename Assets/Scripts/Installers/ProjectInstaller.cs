using GameLoop.Internal;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private MonoGameLoop _gameLoop;
    [SerializeField] private Scenes _scenes;
    [SerializeField] private Texture2D _cursor;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Cursor.SetCursor(_cursor, new Vector2(_cursor.width/2.0f, _cursor.height/2.0f), CursorMode.ForceSoftware);
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
        Container.BindInterfacesAndSelfTo<CoroutinePool>().AsSingle();
    }
}
