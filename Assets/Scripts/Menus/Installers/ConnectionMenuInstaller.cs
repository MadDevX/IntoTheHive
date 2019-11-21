using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

class ConnectionMenuInstaller : MonoInstaller
{
    [SerializeField] private Button _serverButton;
    [SerializeField] private Button _joinButton;
    
    [SerializeField] private Button _backButton;
    [SerializeField] private InputField _ipAddressInputField;
    [SerializeField] private InputField _portNumberInputField;

    public override void InstallBindings()
    {
        Container.BindInstance(_ipAddressInputField).WithId(Identifiers.ConnetionMenuIpInputField);
        Container.BindInstance(_portNumberInputField).WithId(Identifiers.ConnetionMenuPortInputField);
        Container.BindInstance(_serverButton).WithId(Identifiers.ConnetionMenuCreateServerButton);
        Container.BindInstance(_backButton).WithId(Identifiers.ConnetionMenuBackButton);
        Container.BindInstance(_joinButton).WithId(Identifiers.ConnetionMenuJoinServerButton);

        Container.BindInterfacesAndSelfTo<ConnectionMenuManager>().AsSingle();
        //TODO MG get these things out of here this is supposed to be ConnectionMenu Only installer
        Container.BindInterfacesAndSelfTo<LobbyState>().AsSingle();
        Container.BindInterfacesAndSelfTo<ConnectionMenuClientManager>().AsSingle(); 
        Container.BindInterfacesAndSelfTo<NetworkedClientInitializer>().AsSingle();
        Container.BindInterfacesAndSelfTo<ConnectionMenuMessageSender>().AsSingle();
        
    }
}

