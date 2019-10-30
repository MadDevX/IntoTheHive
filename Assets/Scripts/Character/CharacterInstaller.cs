﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CharacterInstaller : MonoInstaller
{
    [SerializeField] private Rigidbody2D _rb;

    public override void InstallBindings()
    {
        InstallComponents();
        InstallCharacter();
        InstallWeapon();
    }

    private void InstallCharacter()
    {
        Container.BindInterfacesAndSelfTo<CharacterMovement>().AsSingle();
        Container.BindInterfacesAndSelfTo<CharacterRotation>().AsSingle();
        Container.BindInterfacesAndSelfTo<CharacterShooting>().AsSingle();
        Container.BindInterfacesAndSelfTo<CharacterEquipment>().AsSingle();

        Container.Bind<ControlState>().AsSingle();
    }

    private void InstallComponents()
    {
        Container.Bind<Rigidbody2D>().FromInstance(_rb).AsSingle();
        Container.Bind<Transform>().FromInstance(transform).AsSingle();
    }

    private void InstallWeapon() //fast solution - delete as soon as possible
    {
        Container.BindInterfacesAndSelfTo<PlaceholderWeapon>().AsSingle();
    }
}
