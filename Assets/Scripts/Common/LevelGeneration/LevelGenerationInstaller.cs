﻿using System;
using UnityEngine;
using Zenject;

public class LevelGenerationInstaller: MonoInstaller
{
    [SerializeField] private Rooms _rooms;

    public override void InstallBindings()
    {
        BindComponents();
        BindFactory();
        BindMessaging();        
    }

    private void BindComponents()
    {
        //TODO MG : make some mechanic that allows for interscene communication without. Maybe a signal bus?
        //Also move classes from ProjectNetworkInstaller here 
        Container.Bind<Rooms>().FromInstance(_rooms).AsSingle();
        Container.BindInterfacesAndSelfTo<LevelSpawner>().AsSingle();
        Container.BindInterfacesAndSelfTo<SpawnParametersGenerator>().AsSingle();
    }

    private void BindFactory()
    {
        Container.BindFactory<RoomSpawnParameters, RoomFacade, RoomFacade.Factory>().FromFactory<RoomFactory>();
    }

    private void BindMessaging()
    {
        Container.BindInterfacesAndSelfTo<LevelGraphMessageReceiver>().AsSingle();
        
    }
}