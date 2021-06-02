using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "GameResources/PlayerSettings")]
class PlayerSettingsInstaller : ScriptableObjectInstaller
{
    [SerializeField] private PlayerOptions _playerOptions;

    public override void InstallBindings()
    {
        Container.BindInstance(_playerOptions).AsSingle();
    }
}