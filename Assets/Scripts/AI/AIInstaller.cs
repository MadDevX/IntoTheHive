﻿using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Pathfinding;
using UnityEngine;
using Zenject;

public class AIInstaller : MonoInstaller
{
    [SerializeField] private AIPath _aiPath;
    [SerializeField] private AIDestinationSetter _aiDestinationSetter;
    public override void InstallBindings()
    {
        InstallAI();
        InstallComponents();
    }

    private void InstallComponents()
    {
        Container.BindInstance(_aiPath).AsSingle();
        Container.BindInstance(_aiDestinationSetter).AsSingle();
    }

    private void InstallAI()
    {

        Container.BindInterfacesAndSelfTo<AIDestinationPointScanner>().AsSingle();
        Container.BindInterfacesAndSelfTo<AITargetForwarder>().AsSingle();
        Container.BindInterfacesAndSelfTo<AIShooterScanner>().AsSingle();
        Container.BindInterfacesAndSelfTo<AITargetScanner>().AsSingle();
        Container.BindInterfacesAndSelfTo<MovementManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<DirectionManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<AILootCreator>().AsSingle();
        Container.BindInterfacesAndSelfTo<AIHPScalingSettings>().AsSingle();
    }
}
