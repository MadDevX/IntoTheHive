using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class MainMenuManager: IInitializable, IDisposable
{
    private Button _singlePlayerButton;
    private Button _multiplayerButton;
    private Button _quitButton;
    public MainMenuManager(
        [Inject(Id = Identifiers.MainMenuSinglePlayerButton)] 
        Button singlePlayerButton,
        [Inject(Id = Identifiers.MainMenuMultiplayerButton)]
        Button multiplayerButton,
        [Inject(Id = Identifiers.MainMenuQuitButton)]
        Button quitButton)
    {
        _singlePlayerButton = singlePlayerButton;
        _multiplayerButton = multiplayerButton;
        _quitButton = quitButton;
    }

    public void Initialize()
    {
        _singlePlayerButton.onClick.AddListener(LaunchSinglePlayer);
        _multiplayerButton.onClick.AddListener(LaunchMultiplayer);
        _quitButton.onClick.AddListener(QuitGame);
    }

    public void Dispose()
    {
        _singlePlayerButton.onClick.RemoveListener(LaunchSinglePlayer);
        _multiplayerButton.onClick.RemoveListener(LaunchMultiplayer);
        _quitButton.onClick.RemoveListener(QuitGame);
    }

    public void LaunchSinglePlayer()
    {
        Debug.Log("Single");
    }

    public void LaunchMultiplayer()
    {
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

