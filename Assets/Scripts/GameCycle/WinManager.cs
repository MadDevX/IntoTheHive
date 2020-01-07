using UnityEngine;

public class WinManager
{
    private int counter = 0;
    private Settings _settings;
    private HostSceneManager _sceneManager;
    private IGameCycleController _gameCycle;

    public WinManager(
        Settings settings,
        HostSceneManager sceneManager,
        IGameCycleController gameCycle
        )
    {
        _settings = settings;
        _sceneManager = sceneManager;
        _gameCycle = gameCycle;
    }

    public void IncreaseCounter()
    {
        counter = counter + 1;
        Debug.Log("ctr= " + counter);
        Debug.Log("sett= " + _settings.levelsToWin);
        if(counter >= _settings.levelsToWin)
        {
            _sceneManager.LoadLobby();
            _gameCycle.RaiseOnGameWon();
        }
        else
        {
            _sceneManager.LoadHub();

        }
    }

    [System.Serializable]
    public class Settings
    {
        public int levelsToWin = 2;
    }
}