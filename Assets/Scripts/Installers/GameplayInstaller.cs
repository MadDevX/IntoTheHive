﻿using Cinemachine;
using DarkRift.Client.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private Camera _mainCamera; 
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private SceneInitializedAnnouncer _announcer;
    [SerializeField] private Transform _hud;
    [SerializeField] private FloatingText _floatingText;
    [SerializeField] private SceneGameplayProperties.Settings _deathSettings;
    [SerializeField] private AnimationControllers _animationControllers;
    public override void InstallBindings()
    {       
        InstallCameras();
        InstallInitializationHandling();
        InstallSpawning();
        InstallItems();
        InstallCharacterBehaviour();
        InstallHUD();
    }

    private void InstallCharacterBehaviour()
    {
        Container.BindInstance(_deathSettings).AsSingle();
        Container.BindInterfacesAndSelfTo<SceneGameplayProperties>().AsSingle();
    }

    private void InstallCameras()
    {
        Container.Bind<Camera>().FromInstance(_mainCamera).AsSingle();
        Container.Bind<CinemachineVirtualCamera>().FromInstance(_virtualCamera).AsSingle();
        Container.BindInterfacesAndSelfTo<CameraManager>().AsSingle();
    }

    private void InstallInitializationHandling()
    {
        Container.BindInstance(_announcer).AsSingle();
        Container.BindInterfacesAndSelfTo<SceneInitializedBaseHandler>().AsSingle();
    }
    private void InstallSpawning()
    {
        Container.BindInstance(_animationControllers);
        Container.BindInterfacesAndSelfTo<CharacterSpawner>().AsSingle();
        Container.BindInterfacesAndSelfTo<CharacterAISpawner>().AsSingle();
        Container.BindInterfacesAndSelfTo<NetworkedCharacterSpawner>().AsSingle();
        Container.BindInterfacesAndSelfTo<NetworkedAISpawner>().AsSingle();
        Container.BindInterfacesAndSelfTo<ItemInitializer>().AsSingle();
    }

    private void InstallItems()
    {
        Container.BindInterfacesAndSelfTo<ModuleFactory>().AsSingle().When(x => x.Container == Container);
        Container.BindInterfacesAndSelfTo<ItemFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<WeaponCreator>().AsSingle();
        Container.BindInterfacesAndSelfTo<PickupManager>().AsSingle();
    }

    private void InstallHUD()
    {
        Container.Bind<Transform>().WithId(Identifiers.HUD).FromInstance(_hud).AsCached();
        Container.BindMonoPrefabPool<FloatingText, FloatingTextSpawnParameters, FloatingText.Factory, FloatingTextPool>(Identifiers.HUD, 10, _floatingText, _hud);
    }

    public class FloatingTextPool : MonoPoolableMemoryPool<FloatingTextSpawnParameters, IMemoryPool, FloatingText>
    {
    }
}
