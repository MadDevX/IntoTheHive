using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LobbyInstaller : MonoInstaller
{
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _readyButton;
    [SerializeField] private Button _leaveLobbyButton;

    public override void InstallBindings()
    {
        Container.BindInstance(_startGameButton).WithId(Identifiers.LobbyStartGameButton);
        Container.BindInstance(_readyButton).WithId(Identifiers.LobbyReadyButton);
        Container.BindInstance(_leaveLobbyButton).WithId(Identifiers.LobbyLeaveButton);

        Container.BindInterfacesAndSelfTo<LobbyManager>().AsSingle();
    }
}

