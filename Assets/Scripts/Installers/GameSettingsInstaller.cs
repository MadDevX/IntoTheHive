using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "GameResources/GameSettings")]
public class GameSettingsInstaller : ScriptableObjectInstaller
{
    [SerializeField] private CharacterMovement.Settings _charSettings;
    public override void InstallBindings()
    {
        Container.BindInstance(_charSettings).AsSingle();
    }
}
