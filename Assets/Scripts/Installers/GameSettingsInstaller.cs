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
    [SerializeField] private RayProjectileRaycaster.Settings _raycasterSettings;
    [SerializeField] private RayProjectileVFX.Settings _rayVFXSettings;
    [SerializeField] private ProjectileDamage.Settings _projectileDamageSettings;
    [SerializeField] private AIShooterScanner.Settings _aiShooterScannerSettings;
    [SerializeField] private AITargetInSight.Settings _aiTargetInSightSettings;
    [SerializeField] private AIShootOnTimer.Settings _aiShootOnTimerSettings;
    [SerializeField] private DirectionManager.Settings _directionManagerSettings;
    [SerializeField] private MovementManager.Settings _movementManagerSettings;
    [SerializeField] private InputMessageSender.Settings _inputSenderSettings;
    [SerializeField] private SpawnParametersGenerator.Settings _roomSpawnSettings;

    public override void InstallBindings()
    {
        Container.BindInstance(_roomSpawnSettings).AsSingle();
        Container.BindInstance(_inputSenderSettings).AsSingle();
        Container.BindInstance(_charMovementSettings).AsSingle();
        Container.BindInstance(_charShootingSettings).AsSingle();
        Container.BindInstance(_placeholderWeaponSettings).AsSingle();
        Container.BindInstance(_aiTargetScannerSettings).AsSingle();
        Container.BindInstance(_raycasterSettings).AsSingle();
        Container.BindInstance(_rayVFXSettings).AsSingle();
        Container.BindInstance(_projectileDamageSettings).AsSingle();
        Container.BindInstance(_aiShooterScannerSettings).AsSingle();
        Container.BindInstance(_aiTargetInSightSettings).AsSingle();
        Container.BindInstance(_aiShootOnTimerSettings).AsSingle();
        Container.BindInstance(_directionManagerSettings).AsSingle();
        Container.BindInstance(_movementManagerSettings).AsSingle();
    }
}
