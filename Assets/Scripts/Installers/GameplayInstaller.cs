using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private Camera _mainCamera;

    public override void InstallBindings()
    {
        Container.Bind<Camera>().FromInstance(_mainCamera).AsSingle();
    }
}
