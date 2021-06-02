using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

/// <summary>
/// Installs all dependencies needed by GameEndedMenuManager
/// </summary>
public class GameEndedMenuInstaller : MonoInstaller
{
    [SerializeField] private Button _hostOkButton;
    [SerializeField] private Button _leaveServerButton;
    [SerializeField] private TextMeshProUGUI _winText;
    [SerializeField] private TextMeshProUGUI _loseText;
    [SerializeField] private SceneInitializedAnnouncer _announcer;

    public override void InstallBindings()
    {
        InstallUIComponents();
        InstallLogic();
        InstallInitialization();
    }
    private void InstallUIComponents()
    {
        Container.BindInstance(_hostOkButton).WithId(Identifiers.GameEndedHostOkButton);
        Container.BindInstance(_leaveServerButton).WithId(Identifiers.GameEndedLeaveServerButton);
        Container.BindInstance(_winText).WithId(Identifiers.GameEndedWinText);
        Container.BindInstance(_loseText).WithId(Identifiers.GameEndedLoseText);
    }

    private void InstallLogic()
    {
        Container.BindInterfacesAndSelfTo<GameEndedMenuManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameEndedMenuInitializer>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameEndedMenuReceiver>().AsSingle();
    }

    private void InstallInitialization()
    {
        Container.BindInstance(_announcer).AsSingle();
        Container.BindInterfacesAndSelfTo<SceneInitializedBaseHandler>().AsSingle();
    }
}
