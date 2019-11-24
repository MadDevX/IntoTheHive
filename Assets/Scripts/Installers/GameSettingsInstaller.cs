using System.Collections;
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
    [SerializeField] private AIFollowInput.Settings _aiFollowInputSettings;

    public override void InstallBindings()
    {
        Container.BindInstance(_charMovementSettings).AsSingle();
        Container.BindInstance(_charShootingSettings).AsSingle();
        Container.BindInstance(_placeholderWeaponSettings).AsSingle();
        Container.BindInstance(_aiTargetScannerSettings).AsSingle();
        Container.BindInstance(_raycasterSettings).AsSingle();
        Container.BindInstance(_rayVFXSettings).AsSingle();
        Container.BindInstance(_projectileDamageSettings).AsSingle();
        Container.BindInstance(_aiShooterScannerSettings).AsSingle();
        Container.BindInstance(_aiFollowInputSettings).AsSingle();
    }
}
