using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private Rigidbody2D _rb;
    public override void InstallBindings()
    {
        InstallCharacter();
        InstallPlayer();
        InstallComponents();
    }

    private void InstallCharacter()
    {
        Container.BindInterfacesAndSelfTo<CharacterMovement>().AsSingle();
        Container.BindInterfacesAndSelfTo<CharacterRotation>().AsSingle();
        Container.Bind<ControlState>().AsSingle();
    }

    private void InstallPlayer()
    {
        Container.BindInterfacesAndSelfTo<PlayerInput>().AsSingle();
    }

    private void InstallComponents()
    {
        Container.Bind<Rigidbody2D>().FromInstance(_rb).AsSingle();
        Container.Bind<Transform>().FromInstance(transform).AsSingle();
    }
}
