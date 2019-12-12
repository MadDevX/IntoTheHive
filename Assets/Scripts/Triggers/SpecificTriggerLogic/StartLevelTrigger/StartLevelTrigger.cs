/// <summary>
/// This class handles trigger invocation from TriggerCounter.
/// </summary>
public class StartLevelTrigger
{
    private HostSceneManager _sceneManager;

    public StartLevelTrigger(
        HostSceneManager sceneManager)
    {
        _sceneManager = sceneManager;
    }

    public void Trigger()
    {
        _sceneManager.LoadNextLevel();
    }
}