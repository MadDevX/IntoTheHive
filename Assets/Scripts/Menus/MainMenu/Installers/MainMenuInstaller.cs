using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuInstaller : MonoInstaller
{
    [SerializeField] private Button _aboutButton;
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _quitGameButton;

    public override void InstallBindings()
    {
        Container.BindInstance(_aboutButton).WithId(Identifiers.MainMenuAboutButton);
        Container.BindInstance(_playButton).WithId(Identifiers.MainMenuPlayButton);
        Container.BindInstance(_quitGameButton).WithId(Identifiers.MainMenuQuitButton);

        Container.BindInterfacesAndSelfTo<MainMenuManager>().AsSingle();
    }
}
