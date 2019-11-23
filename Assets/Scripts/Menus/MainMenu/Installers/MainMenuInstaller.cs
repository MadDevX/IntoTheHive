using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuInstaller : MonoInstaller
{
    [SerializeField] private Button _singlePlayerButton;
    [SerializeField] private Button _multiplayerButton;
    [SerializeField] private Button _quitGameButton;

    public override void InstallBindings()
    {
        Container.BindInstance(_singlePlayerButton).WithId(Identifiers.MainMenuSinglePlayerButton);
        Container.BindInstance(_multiplayerButton).WithId(Identifiers.MainMenuMultiplayerButton);
        Container.BindInstance(_quitGameButton).WithId(Identifiers.MainMenuQuitButton);

        Container.BindInterfacesAndSelfTo<MainMenuManager>().AsSingle();
    }
}
