using System;
using UnityEngine;
using Zenject;

public class WinManager : IInitializable, IDisposable
{
    public int CurrentLevel => _completedLevels + 1;
    private int _completedLevels = 0;
    private Settings _settings;
    private HostSceneManager _sceneManager;
    private IGameCycleController _gameCycleController;
    private IGameCycle _gameCycle;

    public WinManager(
        Settings settings,
        HostSceneManager sceneManager,
        IGameCycleController gameCycleController,
        IGameCycle gameCycle
        )
    {
        _settings = settings;
        _sceneManager = sceneManager;
        _gameCycleController = gameCycleController;
        _gameCycle = gameCycle;
    }

    public void Initialize()
    {
        _gameCycle.OnGameEnded += ResetCounter;
    }

    public void Dispose()
    {
        _gameCycle.OnGameEnded -= ResetCounter;
    }

    public void IncreaseCounter()
    {
        _completedLevels = _completedLevels + 1;
        Debug.Log("ctr= " + _completedLevels);
        Debug.Log("sett= " + _settings.levelsToWin);
        if(_completedLevels >= _settings.levelsToWin)
        {
            _sceneManager.LoadLobby();
            _gameCycleController.RaiseOnGameWon();
        }
        else
        {
            _sceneManager.LoadHub();

        }
    }

    private void ResetCounter()
    {
        _completedLevels = 0;
    }

    [System.Serializable]
    public class Settings
    {
        public int levelsToWin = 2;
    }
}