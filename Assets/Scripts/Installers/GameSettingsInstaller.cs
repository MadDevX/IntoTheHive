﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "GameResources/GameSettings")]
public class GameSettingsInstaller : ScriptableObjectInstaller
{
    [SerializeField] private CharacterMovement.Settings _charMovementSettings;
    [SerializeField] private CharacterShooting.Settings _charShootingSettings;
    [SerializeField] private PlaceholderWeapon.Settings _placeholderWeaponSettings;
    [SerializeField] private AITargetScanner.Settings _aiTargetScannerSettings;
    [SerializeField] private RayProjectile.Settings _raySettings;
    public override void InstallBindings()
    {
        Container.BindInstance(_charMovementSettings).AsSingle();
        Container.BindInstance(_charShootingSettings).AsSingle();
        Container.BindInstance(_placeholderWeaponSettings).AsSingle();
        Container.BindInstance(_aiTargetScannerSettings).AsSingle();
        Container.BindInstance(_raySettings).AsSingle();
    }
}
