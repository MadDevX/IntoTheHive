using System;
using Assets.Scripts.Music;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class MainMenuManager: IInitializable, IDisposable
{
    private Button _playButton;
    private Button _quitButton;
    private AudioManager _audioManager;

    public MainMenuManager(
        [Inject(Id = Identifiers.MainMenuPlayButton)]
        Button multiplayerButton,
        [Inject(Id = Identifiers.MainMenuQuitButton)]
        Button quitButton,
        AudioManager audioManager)
    {
        _playButton = multiplayerButton;
        _quitButton = quitButton;
        _audioManager = audioManager;
    }

    public void Initialize()
    {
        _playButton.onClick.AddListener(LaunchMultiplayer);
        _quitButton.onClick.AddListener(QuitGame);
        _audioManager.PlayIfNotPlaying(Sound.BushWeek);
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

