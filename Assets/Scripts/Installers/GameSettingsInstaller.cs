using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "GameResources/GameSettings")]
public class GameSettingsInstaller : ScriptableObjectInstaller
{
    [SerializeField] private CharacterMovement.Settings _charMovementSettings;
    [SerializeField] private CharacterShooting.Settings _charShootingSettings;
    [SerializeField] private Weapon.Settings _placeholderWeaponSettings;
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
    [SerializeField] private CharacterHealth.Settings _characterHealthSettings;
    [SerializeField] private CharacterSpawner.Settings _characterSpawnerSettings;
    [SerializeField] private AIGraphSpawner.Settings _levelAIGraphSettings;
    [SerializeField] private LevelGraphGenerator.Settings _levelGraphGeneratorSettings;
    [SerializeField] private WinManager.Settings _winManagerSettings;
    [SerializeField] private ExplosionVFX.Settings _explosionVfxSettings;
    [SerializeField] private AIHPScalingSettings.Settings _aiHealthSettings;
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
        Container.BindInstance(_levelGraphGeneratorSettings).AsSingle();
        Container.BindInterfacesAndSelfTo<CharacterHealth.Settings>().FromInstance(_characterHealthSettings).AsSingle();
        Container.BindInstance(_characterSpawnerSettings).AsSingle();
        Container.BindInstance(_levelAIGraphSettings).AsSingle();
        Container.BindInstance(_winManagerSettings).AsSingle();
        Container.BindInstance(_explosionVfxSettings).AsSingle();
        Container.BindInstance(_aiHealthSettings).AsSingle();
    }
}
