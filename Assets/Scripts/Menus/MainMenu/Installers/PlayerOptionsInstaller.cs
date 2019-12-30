using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;
public class PlayerOptionsInstaller: MonoInstaller
{

    [SerializeField] private TextMeshProUGUI _currentNickname;
    [SerializeField] private Text _nicknameTextField;
    [SerializeField] private Button _setNicknameButton;

    public override void InstallBindings()
    {
        InstallUIElements();
        InstallLogic();
    }

    private void InstallLogic()
    {
        Container.BindInterfacesAndSelfTo<PlayerOptionsMenuManager>().AsSingle();
    }

    private void InstallUIElements()
    {
        Container.BindInstance(_currentNickname).WithId(Identifiers.MainMenuCurrentNicknameText);
        Container.BindInstance(_nicknameTextField).WithId(Identifiers.MainMenuNicknameText);
        Container.BindInstance(_setNicknameButton).WithId(Identifiers.MainMenuSetNicknameButton);
    }
}