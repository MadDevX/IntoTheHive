using System;
using UnityEngine.SceneManagement;
using Zenject;

public class SceneInitializedBaseHandler: IInitializable, IDisposable
{
    private ScenePostinitializationEvents _postinitializationEvents;
    private SceneInitializedAnnouncer _announcer;

    public SceneInitializedBaseHandler(
        SceneInitializedAnnouncer announcer,
        ScenePostinitializationEvents postinitializationEvents
        )
    {
        _announcer = announcer;
        _postinitializationEvents = postinitializationEvents;
    }

    public void Initialize()
    {
        _announcer.SceneInitialized += HandleSceneInitialized;
    }

    public void Dispose()
    {
        _announcer.SceneInitialized -= HandleSceneInitialized;
    }

    private void HandleSceneInitialized()
    {
        _postinitializationEvents.InvokeEvents(SceneManager.GetActiveScene().buildIndex);   
    }
}
