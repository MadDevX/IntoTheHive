using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class MainMenuManager: IInitializable, IDisposable
{
    private Button _playButton;
    private Button _quitButton;

    public MainMenuManager(
        [Inject(Id = Identifiers.MainMenuPlayButton)]
        Button multiplayerButton,
        [Inject(Id = Identifiers.MainMenuQuitButton)]
        Button quitButton)
    {
        _playButton = multiplayerButton;
        _quitButton = quitButton;
    }

    public void Initialize()
    {
        _playButton.onClick.AddListener(LaunchMultiplayer);
        _quitButton.onClick.AddListener(QuitGame);
    }

    public void Dispose()
    {
        _playButton.onClick.RemoveListener(LaunchMultiplayer);
        _quitButton.onClick.RemoveListener(QuitGame);
    }

    public void LaunchMultiplayer()
    {
        SceneManager.LoadScene(1);
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

