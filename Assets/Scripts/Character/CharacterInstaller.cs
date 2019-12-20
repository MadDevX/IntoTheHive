using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CharacterInstaller : MonoInstaller
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private CharacterFacade _characterFacade;

    public override void InstallBindings()
    {
        InstallComponents();
        InstallCharacter();
        InstallWeapon();
    }

    private void InstallCharacter()
    {
        Container.BindInterfacesAndSelfTo<CharacterPositionUpdater>().AsSingle();
        Container.BindInterfacesAndSelfTo<CharacterMovement>().AsSingle();
        Container.BindInterfacesAndSelfTo<CharacterRotation>().AsSingle();
        Container.BindInterfacesAndSelfTo<CharacterShooting>().AsSingle();
        Container.BindInterfacesAndSelfTo<CharacterEquipment>().AsSingle();
        Container.BindInterfacesAndSelfTo<CharacterHealth>().AsSingle();
        Container.BindInterfacesAndSelfTo<CharacterRespawnable>().AsSingle();
        Container.BindInterfacesAndSelfTo<CharacterInventory>().AsSingle();
        Container.Bind<ControlState>().AsSingle();
    }

    private void InstallComponents()
    {
        Container.Bind<Rigidbody2D>().FromInstance(_rb).AsSingle();
        Container.Bind<Transform>().FromInstance(transform).AsSingle();
        Container.Bind(typeof(CharacterFacade), typeof(IDisposable)).FromInstance(_characterFacade).AsSingle(); //TODO: check if other bindings were required (if they are - there will be errors)
    }

    private void InstallWeapon() //fast solution - delete as soon as possible
    {
        Container.BindInterfacesAndSelfTo<Weapon>().AsSingle();
    }
}
