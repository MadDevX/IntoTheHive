using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ClientLobbyInstaller : MonoInstaller
{
    [SerializeField] private Button _readyButton;
    [SerializeField] private Button _leaveLobbyButton;

    public override void InstallBindings()
    {
        Container.BindInstance(_readyButton).WithId(Identifiers.LobbyClientReadyButton);
        Container.BindInstance(_leaveLobbyButton).WithId(Identifiers.LobbyClientLeaveButton);
        Container.BindInterfacesAndSelfTo<ClientLobbyManager>().AsSingle();
    }
}



