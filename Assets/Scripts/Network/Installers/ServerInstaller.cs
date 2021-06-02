using DarkRift.Server.Unity;
using UnityEngine;
using Zenject;

class ServerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<ServerManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<CmdServer>().AsSingle();
    }
}

