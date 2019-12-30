using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerOptionsMenuManager: IInitializable, IDisposable
{
    private Text _currentNickname;
    private Text _nicknameTextField;
    private Button _setNicknameButton;

    private PlayerOptions _playerOptions;

    public PlayerOptionsMenuManager(
        [Inject(Id = Identifiers.MainMenuCurrentNicknameText)]
        Text currentNickname,
        [Inject(Id = Identifiers.MainMenuNicknameText)]
        Text nicknameTextField,
        [Inject(Id = Identifiers.MainMenuSetNicknameButton)]
        Button setNicknameButton,
        PlayerOptions playerOptions)
    {
        _currentNickname = currentNickname;
        _nicknameTextField = nicknameTextField;
        _setNicknameButton = setNicknameButton;
        _playerOptions = playerOptions;
    }

    public void Initialize()
    {
        _setNicknameButton.onClick.AddListener(ChangeNickname);
        RefreshNickname();
    }

    public void Dispose()
    {
        _setNicknameButton.onClick.RemoveListener(ChangeNickname);
    }

    private void ChangeNickname()
    {
        var nick = _nicknameTextField.text;
        _playerOptions.Nickname = nick;
        _currentNickname.text = _playerOptions.Nickname;
    }

    private void RefreshNickname()
    {
        _currentNickname.text = _playerOptions.Nickname;
    }

}