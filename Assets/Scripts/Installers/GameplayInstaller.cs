using Cinemachine;
using DarkRift.Client.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private Camera _mainCamera; //Why?
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    public override void InstallBindings()
    {
        Container.Bind<Camera>().FromInstance(_mainCamera).AsSingle();
        Container.Bind<CinemachineVirtualCamera>().FromInstance(_virtualCamera).AsSingle();
    }
}
