using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

class LobbyInstaller: MonoInstaller
{
    [SerializeField] private Button _serverButton;
    [SerializeField] private Button _joinButton;
    [SerializeField] private InputField _ipAddressInputField;
    [SerializeField] private InputField _portNumberInputField;

    public override void InstallBindings()
    {
        Container.BindInstance(_ipAddressInputField).WithId(Identifiers.IpInputField);
        Container.BindInstance(_portNumberInputField).WithId(Identifiers.PortInputField);
        Container.BindInstance(_serverButton).WithId(Identifiers.CreateServerButton);
        Container.BindInstance(_joinButton).WithId(Identifiers.JoinServerButton);
        Container.BindInterfacesAndSelfTo<LobbyManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<NetworkedClientInitializer>().AsSingle();
    }
}

