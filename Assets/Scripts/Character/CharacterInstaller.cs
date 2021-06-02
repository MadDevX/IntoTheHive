using Networking.Character;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CharacterInstaller : MonoInstaller
{
    [SerializeField] private SpriteRenderer _characterSprite;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private CharacterFacade _characterFacade;
    [SerializeField] private Animator _animator;

    public override void InstallBindings()
    {
        InstallComponents();
        InstallCharacter();
        InstallWeapon();
        InstallNetworkedDespawn();
        InstallSpriteManagment();
    }
  
    private void InstallCharacter()
    {
        Container.BindInterfacesAndSelfTo<CharacterMovement>().AsSingle();
        Container.BindInterfacesAndSelfTo<CharacterRotation>().AsSingle();
        Container.BindInterfacesAndSelfTo<CharacterShooting>().AsSingle();
        Container.BindInterfacesAndSelfTo<CharacterHealth>().AsSingle();
        Container.BindInterfacesAndSelfTo<CharacterDamageable>().AsSingle();
        Container.BindInterfacesAndSelfTo<CharacterRespawnable>().AsSingle();
        Container.BindInterfacesAndSelfTo<CharacterInventory>().AsSingle();
        Container.BindInterfacesAndSelfTo<CharacterInfo>().AsSingle();
        Container.Bind<ControlState>().AsSingle();
    }

    private void InstallSpriteManagment()
    {
        Container.BindInterfacesAndSelfTo<CharacterSpriteFixedRotation>().AsSingle();
        Container.BindInterfacesAndSelfTo<CharacterAnimationManager>().AsSingle();
    }

    private void InstallComponents()
    {
        Container.Bind<SpriteRenderer>().FromInstance(_characterSprite).AsSingle();
        Container.Bind<Rigidbody2D>().FromInstance(_rb).AsSingle();
        Container.Bind<Transform>().FromInstance(transform).AsSingle();
        Container.Bind<Animator>().FromInstance(_animator).AsSingle();
        Container.Bind(typeof(CharacterFacade), typeof(IDisposable)).FromInstance(_characterFacade).AsSingle();
    }

    private void InstallWeapon()
    {
        Container.BindInterfacesAndSelfTo<Weapon>().AsSingle();
    }

    private void InstallNetworkedDespawn()
    {
        Container.BindInterfacesAndSelfTo<DeathRequestHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<DeathRequestSender>().AsSingle();
        Container.BindInterfacesAndSelfTo<DisposeCharacterHandler>().AsSingle();
    }
}
