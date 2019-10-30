using DarkRift.Server.Unity;
using UnityEngine;
using Zenject;

class ServerInstaller : MonoInstaller
{
    [SerializeField] private XmlUnityServer _server;

    public override void InstallBindings()
    {
        Container.Bind<XmlUnityServer>().FromInstance(_server).AsSingle();
        Container.BindInterfacesAndSelfTo<ServerManager>().AsSingle();
    }
}

