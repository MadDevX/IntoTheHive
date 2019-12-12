using UnityEngine;
/// <summary>
/// In this class synchronized scene changes are stores with their corresponding events so that other classes do not need to know which event to fire.
/// </summary>
public class HostSceneManager
{
    private SynchronizedSceneManager _synchronizedSceneManager;
    private ProjectEventManager _eventManager;

    public HostSceneManager(
        SynchronizedSceneManager synchronizedSceneManager,
        ProjectEventManager eventManager)
    {
        _eventManager = eventManager;
        _synchronizedSceneManager = synchronizedSceneManager;
    }

    /// <summary>
    /// Sends a synchronized message to all clients and fires GameInitializedHost which is used by GameplayInitializer.
    /// </summary>
    public void LoadNextLevel()
    {
        Debug.Log(this.ToString() + ": loading next level");
        _synchronizedSceneManager.SendSceneChanged(3, _eventManager.FireGameInitializedHost);
    }

    public void LoadHub()
    {

    }

    public void LoadMenu()
    {

    }

}

